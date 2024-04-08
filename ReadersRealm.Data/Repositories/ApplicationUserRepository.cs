namespace ReadersRealm.Data.Repositories;

using Contracts;
using Microsoft.EntityFrameworkCore;
using Models;

public class ApplicationUserRepository(ReadersRealmDbContext dbContext)
    : Repository<ApplicationUser>(dbContext), IApplicationUserRepository
{
    private readonly ReadersRealmDbContext _dbContext = dbContext;

    public async Task<ApplicationUser?> GetByIdAsync(Guid id)
    {
        return await this
            ._dbContext
            .ApplicationUsers
            .FirstOrDefaultAsync(applicationUser => applicationUser.Id.Equals(id));
    }
}