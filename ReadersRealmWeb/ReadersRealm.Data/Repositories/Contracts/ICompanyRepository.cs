namespace ReadersRealm.Data.Repositories.Contracts;

using Models;

public interface ICompanyRepository : IRepository<Company>
{
    Task<Company?> GetByIdAsync(Guid id);
}