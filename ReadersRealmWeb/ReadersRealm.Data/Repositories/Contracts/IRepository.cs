namespace ReadersRealm.Data.Repositories.Contracts;

using System.Linq.Expressions;
using Models.Contracts;

public interface IRepository<TEntity, TId> where TEntity : class, IReadersRealmDbContextBaseEntityModel<TId>
{
    Task<List<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy,
        string includeProperties);
    Task<TEntity?> GetByIdAsync(TId id);
    Task AddAsync(TEntity entity);
    void Update(TEntity entity);
    void Delete(TEntity entity);
    void DeleteRange(IEnumerable<TEntity> entities);
}