namespace ReadersRealm.Services.Data.CategoryServices;

using Common.Exceptions.Category;
using Contracts;
using ReadersRealm.Data.Models;
using ReadersRealm.Data.Repositories.Contracts;
using Web.ViewModels.Category;

public class CategoryCrudService(IUnitOfWork unitOfWork) : ICategoryCrudService
{
    public async Task CreateCategoryAsync(CreateCategoryViewModel categoryModel)
    {
        Category categoryToAdd = new Category()
        {
            Name = categoryModel.Name,
            DisplayOrder = categoryModel.DisplayOrder,
        };

        await unitOfWork
            .CategoryRepository
            .AddAsync(categoryToAdd);

        await unitOfWork.SaveAsync();
    }

    public async Task EditCategoryAsync(EditCategoryViewModel categoryModel)
    {
        Category? categoryToEdit = await unitOfWork
            .CategoryRepository
            .GetByIdAsync(categoryModel.Id);

        if (categoryToEdit == null)
        {
            throw new CategoryNotFoundException();
        }

        categoryToEdit.Name = categoryModel.Name;
        categoryToEdit.DisplayOrder = categoryModel.DisplayOrder;

        await unitOfWork
            .SaveAsync();
    }

    public async Task DeleteCategoryAsync(DeleteCategoryViewModel categoryModel)
    {
        Category? categoryToDelete = await unitOfWork
            .CategoryRepository
            .GetByIdAsync(categoryModel.Id);

        if (categoryToDelete == null)
        {
            throw new CategoryNotFoundException();
        }

        unitOfWork
            .CategoryRepository
            .Delete(categoryToDelete);

        await unitOfWork
            .SaveAsync();
    }
}