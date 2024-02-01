namespace ReadersRealm.Services;

using Contracts;
using Data;
using Data.Models;
using Data.Repositories;
using Data.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using Web.ViewModels.Category;

public class CategoryService : ICategoryService
{
    private readonly ReadersRealmDbContext dbContext;
    private readonly IUnitOfWork unitOfWork;

    public CategoryService(ReadersRealmDbContext dbContext, IUnitOfWork unitOfWork)
    {
        this.dbContext = dbContext;
        this.unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<AllCategoriesViewModel>> GetAllAsync()
    {
        // IEnumerable<AllCategoriesViewModel> allCategories = await this
        //     .dbContext
        //     .Categories
        //     .AsNoTracking()
        //     .Select(c => new AllCategoriesViewModel()
        //     {
        //         Id = c.Id,
        //         Name = c.Name,
        //         DisplayOrder = c.DisplayOrder,
        //     })
        //     .ToListAsync();

        List<Category> allCategories = await this
            .unitOfWork
            .CategoryRepository
            .GetAsync(null, null, "");

        IEnumerable<AllCategoriesViewModel> categoriesToReturn = allCategories
            .Select(c => new AllCategoriesViewModel()
            {
                Id = c.Id,
                Name = c.Name,
                DisplayOrder = c.DisplayOrder,
            });

        return categoriesToReturn;
    }

    public async Task CreateCategoryAsync(CreateCategoryViewModel categoryModel)
    {
        Category categoryToAdd = new Category()
        {
            Name = categoryModel.Name,
            DisplayOrder = categoryModel.DisplayOrder,
        };

        // await this
        //     .dbContext
        //     .Categories
        //     .AddAsync(categoryToAdd);

        await this
            .unitOfWork
            .CategoryRepository
            .AddAsync(categoryToAdd);

        // await this.dbContext.SaveChangesAsync();
        await this.unitOfWork.SaveAsync();
    }

    public async Task<Category?> GetCategoryByIdAsync(int id)
    {
        // return await this
        //     .dbContext
        //     .Categories
        //     .AsNoTracking()
        //     .FirstOrDefaultAsync(c => c.Id == id);

        return await this
            .unitOfWork
            .CategoryRepository
            .GetByIdAsync(id);
    }

    public async Task EditCategoryAsync(EditCategoryViewModel categoryModel)
    {
        // Category categoryToEdit = await this
        //     .dbContext
        //     .Categories
        //     .FirstAsync(c => c.Id == categoryModel.Id);

        Category categoryToEdit = await this
            .unitOfWork
            .CategoryRepository
            .GetByIdAsync(categoryModel.Id);

        categoryToEdit.Name = categoryModel.Name;
        categoryToEdit.DisplayOrder = categoryModel.DisplayOrder;

        // await this
        //     .dbContext
        //     .SaveChangesAsync();

        await this.unitOfWork.SaveAsync();
    }

    public async Task DeleteCategoryAsync(DeleteCategoryViewModel categoryModel)
    {
        // Category categoryToDelete = await this
        //     .dbContext
        //     .Categories
        //     .FirstAsync(c => c.Id == categoryModel.Id);

        Category categoryToDelete = await this
            .unitOfWork
            .CategoryRepository
            .GetByIdAsync(categoryModel.Id);

        this.unitOfWork.CategoryRepository.Delete(categoryToDelete);

        // this
        //     .dbContext
        //     .Categories
        //     .Remove(categoryToDelete);

        // await this.dbContext.SaveChangesAsync();

        await this.unitOfWork.SaveAsync();
    }

}