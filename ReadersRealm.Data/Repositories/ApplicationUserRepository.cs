﻿namespace ReadersRealm.Data.Repositories;

using Contracts;
using Microsoft.EntityFrameworkCore;
using Models;

public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
{
    private readonly ReadersRealmDbContext _dbContext;
    public ApplicationUserRepository(ReadersRealmDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ApplicationUser?> GetByIdAsync(string id)
    {
        return await _dbContext
            .ApplicationUsers
            .FirstOrDefaultAsync(applicationUser => applicationUser.Id == id);
    }
}