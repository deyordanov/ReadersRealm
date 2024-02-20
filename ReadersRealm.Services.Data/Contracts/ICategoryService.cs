namespace ReadersRealm.Services.Contracts;

using Data.Models;
using ViewModels.Category;

public interface ICategoryService
{
    Task<IEnumerable<AllCategoriesViewModel>> GetAllAsync();
    Task<List<AllCategoriesListViewModel>> GetAllListAsync();
    CreateCategoryViewModel GetCategoryForCreate();
    Task<EditCategoryViewModel> GetCategoryForEditAsync(int id);
    Task<DeleteCategoryViewModel> GetCategoryForDeleteAsync(int id);
    Task CreateCategoryAsync(CreateCategoryViewModel categoryModel);
    Task EditCategoryAsync(EditCategoryViewModel categoryModel);
    Task DeleteCategoryAsync(DeleteCategoryViewModel categoryModel);
}