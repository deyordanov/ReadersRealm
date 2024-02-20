﻿namespace ReadersRealm.Data.Repositories;

using Contracts;
using Microsoft.EntityFrameworkCore;
using Models;

public class AuthorRepository : Repository<Author>, IAuthorRepository
{
    private readonly ReadersRealmDbContext _dbContext;
    public AuthorRepository(ReadersRealmDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Author?> GetByIdAsync(Guid id)
    {
        return await _dbContext
            .Authors
            .FirstAsync(author => author.Id == id);
    }
}