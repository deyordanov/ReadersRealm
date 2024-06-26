﻿namespace ReadersRealm.Data.Repositories;

using System.Linq.Expressions;
using Contracts;
using Microsoft.EntityFrameworkCore;
using Models;

public class CategoryRepository(ReadersRealmDbContext dbContext) : Repository<Category>(dbContext), ICategoryRepository
{
    private readonly ReadersRealmDbContext _dbContext = dbContext;

    public async Task<Category?> GetByIdAsync(int id)
    {
        return await this
            ._dbContext
            .Categories
            .FirstOrDefaultAsync(category => category.Id == id);
    }

    public async Task<Category?> GetFirstOrDefaultByFilterAsync(Expression<Func<Category, bool>> filter, bool tracking)
    {
        if (tracking)
        {
            return await this
                ._dbContext
                .Categories
                .FirstOrDefaultAsync(filter);
        }

        return await this
            ._dbContext
            .Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(filter);
    }
}