namespace ReadersRealm.Data.Repositories.Contracts;

using System.Linq.Expressions;
using Models;

public interface ICategoryRepository : IRepository<Category>
{
    Task<Category?> GetByIdAsync(int id);
    Task<Category?> GetFirstOrDefaultByFilterAsync(Expression<Func<Category, bool>> filter, bool tracking);
}