namespace ReadersRealm.Data.Repositories.Contracts;

using System.Linq.Expressions;
using Models;

public interface ICompanyRepository : IRepository<Company>
{
    Task<Company?> GetByIdAsync(Guid id);
    Task<Company?> GetFirstOrDefaultByFilterAsync(Expression<Func<Company, bool>> filter, bool tracking);
}