namespace ReadersRealm.Services.Data.CategoryServices;

using Common.Exceptions.Category;
using Contracts;
using ReadersRealm.Data.Models;
using ReadersRealm.Data.Repositories.Contracts;
using Web.ViewModels.Category;

public class CategoryRetrievalService : ICategoryRetrievalService
{
    private readonly IUnitOfWork _unitOfWork;

    public CategoryRetrievalService(IUnitOfWork unitOfWork)
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

    public async Task<bool> CategoryExistsAsync(int categoryId)
    {
        return await this
            ._unitOfWork
            .CategoryRepository
            .GetFirstOrDefaultByFilterAsync(category => category.Id == categoryId, false) != null;
    }
}