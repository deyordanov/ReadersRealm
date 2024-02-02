namespace ReadersRealm.Services;

using Common.Exceptions;
using Contracts;
using Data;
using Data.Models;
using Data.Repositories.Contracts;
using ViewModels.Category;

public class CategoryService : ICategoryService
{
    private readonly IUnitOfWork unitOfWork;

    public CategoryService(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<AllCategoriesViewModel>> GetAllAsync()
    {
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

    public async Task<List<AllCategoriesListViewModel>> GetAllListAsync()
    {
        List<Category> allCategories = await this
            .unitOfWork
            .CategoryRepository
            .GetAsync(null, null, "");

        List<AllCategoriesListViewModel> categoriesToReturn = allCategories
            .Select(c => new AllCategoriesListViewModel()
            {
                Id = c.Id,
                Name = c.Name
            })
            .ToList(); ;

        return categoriesToReturn;
    }

    public async Task CreateCategoryAsync(CreateCategoryViewModel categoryModel)
    {
        Category categoryToAdd = new Category()
        {
            Name = categoryModel.Name,
            DisplayOrder = categoryModel.DisplayOrder,
        };

        await this
            .unitOfWork
            .CategoryRepository
            .AddAsync(categoryToAdd);

        await this.unitOfWork.SaveAsync();
    }

    public async Task<Category?> GetCategoryByIdAsync(int id)
    {
        return await this
            .unitOfWork
            .CategoryRepository
            .GetByIdAsync(id);
    }

    public async Task EditCategoryAsync(EditCategoryViewModel categoryModel)
    {
        Category? categoryToEdit = await this
            .unitOfWork
            .CategoryRepository
            .GetByIdAsync(categoryModel.Id);

        if (categoryToEdit == null)
        {
            throw new CategoryNotFoundException();
        }

        categoryToEdit.Name = categoryModel.Name;
        categoryToEdit.DisplayOrder = categoryModel.DisplayOrder;

        await this.unitOfWork.SaveAsync();
    }

    public async Task DeleteCategoryAsync(DeleteCategoryViewModel categoryModel)
    {
        Category? categoryToDelete = await this
            .unitOfWork
            .CategoryRepository
            .GetByIdAsync(categoryModel.Id);

        if (categoryToDelete == null)
        {
            throw new CategoryNotFoundException();
        }

        this
            .unitOfWork
            .CategoryRepository
            .Delete(categoryToDelete);

        await this
            .unitOfWork
            .SaveAsync();
    }

}