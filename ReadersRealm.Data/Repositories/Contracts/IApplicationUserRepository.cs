using ReadersRealm.Data.Models;

namespace ReadersRealm.Data.Repositories.Contracts;

public interface IApplicationUserRepository : IRepository<ApplicationUser>
{
    Task<ApplicationUser?> GetByIdAsync(string id);
}