namespace ReadersRealm.Data.Repositories;

using System.Linq.Expressions;
using Common.Exceptions.EntityProperty;
using Contracts;
using Microsoft.EntityFrameworkCore;

public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{
    private readonly ReadersRealmDbContext _dbContext;
    private readonly DbSet<TEntity> _dbSet;

    protected Repository(ReadersRealmDbContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<TEntity>();
    }

    public Task<List<TEntity>> GetAsync(Expression<Func<TEntity, bool>>? filter, 
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy, 
        string properties)
    {
        IQueryable<TEntity> query = _dbSet.AsNoTracking();

        if (filter != null)
        {
            query = query.Where(filter);
        }

        string[] propertiesToAdd = properties.Split(", ", StringSplitOptions.RemoveEmptyEntries);

        if (!ArePropertiesPresentInEntity(propertiesToAdd))
        {
            throw new PropertyNotFoundException();
        }

        foreach (string property in propertiesToAdd)
        {
            query = query.Include(property);
        }

        if (orderBy != null)
        {
            return orderBy(query).ToListAsync();
        }

        return query.ToListAsync();
    }

    public void DetachEntity(TEntity entity)
    {
        _dbContext.Entry(entity).State = EntityState.Detached;
    }

    public async Task AddAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public void Update(TEntity entity)
    {
        _dbSet.Attach(entity);
        _dbContext.Entry(entity).State = EntityState.Modified;
    }

    public void Delete(TEntity entity)
    {
        _dbSet.Remove(entity);
    }

    public void DeleteRange(IEnumerable<TEntity> entities)
    {
        _dbSet.RemoveRange(entities);
    }

    /// <summary>
    /// Validates whether all specified properties for eager loading are present on the TEntity type.
    /// This method is used to ensure the properties listed in the GetAsync() method call exist on the entity,
    /// avoiding runtime errors during query execution.
    /// </summary>
    /// <param name="propertiesToAdd">An array of property names intended for eager loading.</param>
    /// <returns>True if all specified properties exist on TEntity; otherwise, false. 
    /// This boolean value helps in determining if the GetAsync method can proceed with including these properties in the query.</returns>
    protected bool ArePropertiesPresentInEntity(string[] propertiesToAdd)
    {
        Type entityType = typeof(TEntity);
        List<string> entityProperties = entityType
            .GetProperties()
            .Select(property => property.Name)
            .ToList();

        foreach (string propertyToAdd in propertiesToAdd)
        {
            if (!entityProperties.Contains(propertyToAdd))
            {
                return false;
            }
        }

        return true;
    }
}