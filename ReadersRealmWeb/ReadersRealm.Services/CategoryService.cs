namespace ReadersRealm.Services;

using Contracts;
using Data;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Web.ViewModels.Category;

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
            .AsNoTracking()
            .Select(c => new AllCategoriesViewModel()
            {
                Id = c.Id,
                Name = c.Name,
                DisplayOrder = c.DisplayOrder,
            })
            .ToListAsync();

        return allCategories;
    }

    public async Task CreateCategoryAsync(CreateCategoryViewModel categoryModel)
    {
        Category categoryToAdd = new Category()
        {
            Name = categoryModel.Name,
            DisplayOrder = categoryModel.DisplayOrder,
        };

        await this
            .dbContext
            .Categories
            .AddAsync(categoryToAdd);

        await this.dbContext.SaveChangesAsync();
    }

    public async Task<Category?> GetCategoryByIdAsync(int? id)
    {
        return await this
            .dbContext
            .Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task EditCategoryAsync(EditCategoryViewModel categoryModel)
    {
        Category categoryToEdit = await this
            .dbContext
            .Categories
            .FirstAsync(c => c.Id == categoryModel.Id);

        categoryToEdit.Name = categoryModel.Name;
        categoryToEdit.DisplayOrder = categoryModel.DisplayOrder;

        await this
            .dbContext
            .SaveChangesAsync();
    }

    public async Task DeleteCategoryAsync(DeleteCategoryViewModel categoryModel)
    {
        Category categoryToDelete = await this
            .dbContext
            .Categories
            .FirstAsync(c => c.Id == categoryModel.Id);

        this
            .dbContext
            .Categories
            .Remove(categoryToDelete);

        await this.dbContext.SaveChangesAsync();
    }
}