namespace ReadersRealm.Data.Repositories.Contracts;

using System.Linq.Expressions;
using Models;

public interface IAuthorRepository : IRepository<Author>
{
    Task<Author?> GetByIdAsync(Guid id);
    Task<Author?> GetFirstOrDefaultWithFilterAsync(Expression<Func<Author, bool>> filter, bool tracking);
}