namespace ReadersRealm.Data.Repositories;

using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models;

public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
{
    private readonly ReadersRealmDbContext dbContext;
    public ApplicationUserRepository(ReadersRealmDbContext dbContext) : base(dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<ApplicationUser?> GetByIdAsync(string id)
    {
        return await this
            .dbContext
            .ApplicationUsers
            .FirstOrDefaultAsync(applicationUser => applicationUser.Id == id);
    }
}