namespace ReadersRealm.Services.Contracts;

using DataModels;
using ViewModels.Category;

public interface ICategoryService
{
    Task<IEnumerable<AllCategoriesViewModel>> GetAllAsync();

    Task CreateCategoryAsync(CreateCategoryViewModel categoryModel);

    Task<Category?> GetCategoryById(int? id);

    Task EditCategory(EditCategoryViewModel categoryModel);

    Task DeleteCategory(DeleteCategoryViewModel categoryModel);
}