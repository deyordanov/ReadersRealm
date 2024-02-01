namespace ReadersRealm.Data.Repositories;

using System.Linq.Expressions;
using Contracts;
using Microsoft.EntityFrameworkCore;
using Models.Contracts;

public abstract class Repository<TEntity, TId> : IRepository<TEntity, TId> where TEntity : class, IReadersRealmDbContextBaseEntityModel<TId>
{
    private ReadersRealmDbContext dbContext;
    private DbSet<TEntity> dbSet;

    protected Repository(ReadersRealmDbContext dbContext)
    {
        this.dbContext = dbContext;
        this.dbSet = this.dbContext.Set<TEntity>();
    }

    public Task<List<TEntity>> GetAsync(Expression<Func<TEntity, bool>>? filter, 
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy, 
        string propertiesToInclude)
    {
        IQueryable<TEntity> query = this.dbSet.AsNoTracking();

        if (filter != null)
        {
            query = query.Where(filter);
        }

        foreach (string property in propertiesToInclude
                     .Split(", ", StringSplitOptions.RemoveEmptyEntries))
        {
            query = query.Include(property);
        }

        if (orderBy != null)
        {
            return orderBy(query).ToListAsync();
        }

        return query.ToListAsync();
    }

    public Task<TEntity?> GetByIdAsync(TId id)
    {
        return this.dbSet.FirstOrDefaultAsync(e => e.Id!.Equals(id));
    }

    public async Task AddAsync(TEntity entity)
    {
        await this.dbSet.AddAsync(entity);
    }

    public void Update(TEntity entity)
    {
        this.dbSet.Attach(entity);
        this.dbContext.Entry(entity).State = EntityState.Modified;
    }

    public void Delete(TEntity entity)
    {
        this.dbSet.Remove(entity);
    }

    public void DeleteRange(IEnumerable<TEntity> entities)
    {
        this.dbSet.RemoveRange(entities);
    }
}