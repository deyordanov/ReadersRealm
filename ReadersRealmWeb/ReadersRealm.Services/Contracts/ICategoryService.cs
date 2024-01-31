namespace ReadersRealm.Services.Contracts;

using Data.Models;
using Web.ViewModels.Category;

public interface ICategoryService
{
    Task<IEnumerable<AllCategoriesViewModel>> GetAllAsync();

    Task CreateCategoryAsync(CreateCategoryViewModel categoryModel);

    Task<Category?> GetCategoryByIdAsync(int? id);

    Task EditCategoryAsync(EditCategoryViewModel categoryModel);

    Task DeleteCategoryAsync(DeleteCategoryViewModel categoryModel);
}