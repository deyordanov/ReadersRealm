﻿namespace ReadersRealm.Data.Repositories;

using Common.Exceptions;
using Contracts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{
    private readonly ReadersRealmDbContext _dbContext;
    private readonly DbSet<TEntity> _dbSet;

    protected Repository(ReadersRealmDbContext dbContext)
    {
        this._dbContext = dbContext;
        this._dbSet = this._dbContext.Set<TEntity>();
    }

    public Task<List<TEntity>> GetAsync(Expression<Func<TEntity, bool>>? filter, 
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy, 
        string properties)
    {
        IQueryable<TEntity> query = this._dbSet.AsNoTracking();

        if (filter != null)
        {
            query = query.Where(filter);
        }

        string[] propertiesToAdd = properties.Split(", ", StringSplitOptions.RemoveEmptyEntries);

        if (!this.ArePropertiesPresentInEntity(propertiesToAdd))
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

    public async Task AddAsync(TEntity entity)
    {
        await this._dbSet.AddAsync(entity);
    }

    public void Update(TEntity entity)
    {
        this._dbSet.Attach(entity);
        this._dbContext.Entry(entity).State = EntityState.Modified;
    }

    public void Delete(TEntity entity)
    {
        this._dbSet.Remove(entity);
    }

    public void DeleteRange(IEnumerable<TEntity> entities)
    {
        this._dbSet.RemoveRange(entities);
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