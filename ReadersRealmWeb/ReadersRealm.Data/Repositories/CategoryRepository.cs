namespace ReadersRealm.Data.Repositories;

using Contracts;
using Microsoft.EntityFrameworkCore;
using Models;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    private readonly ReadersRealmDbContext _dbContext;

    public CategoryRepository(ReadersRealmDbContext dbContext)
        : base(dbContext)
    {
        this._dbContext = dbContext;
    }

    public async Task<Category?> GetByIdAsync(int id)
    {
        return await this
            ._dbContext
            .Categories
            .FirstOrDefaultAsync(category => category.Id == id);
    }
}