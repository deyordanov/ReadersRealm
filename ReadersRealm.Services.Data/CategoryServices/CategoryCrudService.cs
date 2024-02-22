namespace ReadersRealm.Services.Data.CategoryServices;

using Common.Exceptions.Category;
using Contracts;
using ReadersRealm.Data.Models;
using ReadersRealm.Data.Repositories.Contracts;
using Web.ViewModels.Category;

public class CategoryCrudService : ICategoryCrudService
{
    private readonly IUnitOfWork _unitOfWork;

    public CategoryCrudService(IUnitOfWork unitOfWork)
    {
        this._unitOfWork = unitOfWork;
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

        await this
            ._unitOfWork.SaveAsync();
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

        await this
            ._unitOfWork
            .SaveAsync();
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

        this._unitOfWork
            .CategoryRepository
            .Delete(categoryToDelete);

        await this
            ._unitOfWork
            .SaveAsync();
    }
}