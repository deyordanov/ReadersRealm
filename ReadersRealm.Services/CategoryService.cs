namespace ReadersRealm.Services.Data;

using Common.Exceptions;
using ReadersRealm.Data.Models;
using ReadersRealm.Data.Repositories.Contracts;
using Contracts;
using ViewModels.Category;

public class CategoryService : ICategoryService
{
    private readonly IUnitOfWork _unitOfWork;

    public CategoryService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<AllCategoriesViewModel>> GetAllAsync()
    {
        List<Category> allCategories = await _unitOfWork
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
        List<Category> allCategories = await _unitOfWork
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
        Category? category = await _unitOfWork
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
        Category? category = await _unitOfWork
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

        await _unitOfWork
            .CategoryRepository
            .AddAsync(categoryToAdd);

        await _unitOfWork.SaveAsync();
    }

    public async Task EditCategoryAsync(EditCategoryViewModel categoryModel)
    {
        Category? categoryToEdit = await _unitOfWork
            .CategoryRepository
            .GetByIdAsync(categoryModel.Id);

        if (categoryToEdit == null)
        {
            throw new CategoryNotFoundException();
        }

        categoryToEdit.Name = categoryModel.Name;
        categoryToEdit.DisplayOrder = categoryModel.DisplayOrder;

        await _unitOfWork.SaveAsync();
    }

    public async Task DeleteCategoryAsync(DeleteCategoryViewModel categoryModel)
    {
        Category? categoryToDelete = await _unitOfWork
            .CategoryRepository
            .GetByIdAsync(categoryModel.Id);

        if (categoryToDelete == null)
        {
            throw new CategoryNotFoundException();
        }

        _unitOfWork
            .CategoryRepository
            .Delete(categoryToDelete);

        await _unitOfWork
            .SaveAsync();
    }

}