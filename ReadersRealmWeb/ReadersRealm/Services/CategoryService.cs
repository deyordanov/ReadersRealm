namespace ReadersRealm.Services;

using Contracts;
using Data;
using Microsoft.EntityFrameworkCore;
using ViewModels.Category;

public class CategoryService : ICategoryService
{
    private readonly ReadersRealmDbContext dbContext;

    public CategoryService(ReadersRealmDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<IEnumerable<AllCategoriesViewModel>> GetAllAsync()
    {
        IEnumerable<AllCategoriesViewModel> allCategories = await this
            .dbContext
            .Categories
            .Select(c => new AllCategoriesViewModel()
            {
                Id = c.Id,
                Name = c.Name,
                DisplayOrder = c.DisplayOrder,
            })
            .ToListAsync();

        return allCategories;
    }
}