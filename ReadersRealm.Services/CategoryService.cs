namespace ReadersRealm.Services;

using Common.Exceptions;
using Contracts;
using Data;
using Data.Models;
using Data.Repositories.Contracts;
using ViewModels.Category;

public class CategoryService : ICategoryService
{
    private readonly IUnitOfWork _unitOfWork;

    public CategoryService(IUnitOfWork unitOfWork)
    {
        this._unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<AllCategoriesViewModel>> GetAllAsync()
    {
        List<Category> allCategories = await this
            ._unitOfWork
            .CategoryRepository
            .GetAsync(null, null, string.Empty);

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
            ._unitOfWork
            .CategoryRepository
            .GetAsync(null, null, string.Empty);

        List<AllCategoriesListViewModel> categoriesToReturn = allCategories
            .Select(c => new AllCategoriesListViewModel()
            {
                Id = c.Id,
                Name = c.Name
            })
            .ToList(); ;

        return categoriesToReturn;
    }

    public async Task<EditCategoryViewModel> GetCategoryForEditAsync(int id)
    {
        Category? category = await this
            ._unitOfWork
            .CategoryRepository
            .GetByIdAsync(id);

        if (category == null)
        {
            throw new CategoryNotFoundException();
        }

        EditCategoryViewModel categoryModel = new EditCategoryViewModel()
        {
            Id = category.Id,
            Name = category.Name,
            DisplayOrder = category.DisplayOrder,
        };
        
        return categoryModel;
    }

    public async Task<DeleteCategoryViewModel> GetCategoryForDeleteAsync(int id)
    {
        Category? category = await this
            ._unitOfWork
            .CategoryRepository
            .GetByIdAsync(id);

        if (category == null)
        {
            throw new CategoryNotFoundException();
        }

        DeleteCategoryViewModel categoryModel = new DeleteCategoryViewModel()
        {
            Id = category.Id,
            Name = category.Name,
            DisplayOrder = category.DisplayOrder,
        };

        return categoryModel;
    }

    public CreateCategoryViewModel GetCategoryForCreate()
    {
        CreateCategoryViewModel categoryModel = new CreateCategoryViewModel()
        {
            Name = string.Empty
        };

        return categoryModel;
    }

    public async Task CreateCategoryAsync(CreateCategoryViewModel categoryModel)
    {
        Category categoryToAdd = new Category()
        {
            Name = categoryModel.Name,
            DisplayOrder = categoryModel.DisplayOrder,
        };

        await this
            ._unitOfWork
            .CategoryRepository
            .AddAsync(categoryToAdd);

        await this._unitOfWork.SaveAsync();
    }

    public async Task EditCategoryAsync(EditCategoryViewModel categoryModel)
    {
        Category? categoryToEdit = await this
            ._unitOfWork
            .CategoryRepository
            .GetByIdAsync(categoryModel.Id);

        if (categoryToEdit == null)
        {
            throw new CategoryNotFoundException();
        }

        categoryToEdit.Name = categoryModel.Name;
        categoryToEdit.DisplayOrder = categoryModel.DisplayOrder;

        await this._unitOfWork.SaveAsync();
    }

    public async Task DeleteCategoryAsync(DeleteCategoryViewModel categoryModel)
    {
        Category? categoryToDelete = await this
            ._unitOfWork
            .CategoryRepository
            .GetByIdAsync(categoryModel.Id);

        if (categoryToDelete == null)
        {
            throw new CategoryNotFoundException();
        }

        this
            ._unitOfWork
            .CategoryRepository
            .Delete(categoryToDelete);

        await this
            ._unitOfWork
            .SaveAsync();
    }

}