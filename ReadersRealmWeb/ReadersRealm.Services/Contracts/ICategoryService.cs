namespace ReadersRealm.Services.Contracts;

using Data.Models;
using ViewModels.Category;

public interface ICategoryService
{
    Task<IEnumerable<AllCategoriesViewModel>> GetAllAsync();
    Task<Category?> GetCategoryByIdAsync(int id);
    Task<List<AllCategoriesListViewModel>> GetAllListAsync();
    Task CreateCategoryAsync(CreateCategoryViewModel categoryModel);
    Task EditCategoryAsync(EditCategoryViewModel categoryModel);
    Task DeleteCategoryAsync(DeleteCategoryViewModel categoryModel);
}