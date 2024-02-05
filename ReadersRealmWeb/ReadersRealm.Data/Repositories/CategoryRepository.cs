namespace ReadersRealm.Data.Repositories;

using Contracts;
using Microsoft.EntityFrameworkCore;
using Models;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    private readonly ReadersRealmDbContext dbContext;

    public CategoryRepository(ReadersRealmDbContext dbContext)
        : base(dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<Category?> GetByIdAsync(int id)
    {
        return await this
            .dbContext
            .Categories
            .FirstOrDefaultAsync(category => category.Id == id);
    }
}