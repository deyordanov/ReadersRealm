namespace ReadersRealm.Data.Repositories.Contracts;

using System.Linq.Expressions;

public interface IRepository<TEntity> where TEntity : class
{
    Task<List<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy,
        string includeProperties);

    void DetachEntity(TEntity entity);
    Task AddAsync(TEntity entity);
    void Update(TEntity entity);
    void Delete(TEntity entity);
    void DeleteRange(IEnumerable<TEntity> entities);
}