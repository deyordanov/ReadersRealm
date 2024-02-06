

using ReadersRealm.Data.Models;
using ReadersRealm.Data.Repositories.Contracts;

public interface IApplicationUserRepository : IRepository<ApplicationUser>
{
    Task<ApplicationUser?> GetByIdAsync(string id);
}