namespace ReadersRealm.Services.Data.CategoryServices.Contracts;

using Web.ViewModels.Category;

public interface ICategoryRetrievalService
{
    Task<IEnumerable<AllCategoriesViewModel>> GetAllAsync();
    Task<List<AllCategoriesListViewModel>> GetAllListAsync();
    CreateCategoryViewModel GetCategoryForCreate();
    Task<EditCategoryViewModel> GetCategoryForEditAsync(int id);
    Task<DeleteCategoryViewModel> GetCategoryForDeleteAsync(int id);
    Task<bool> CategoryExistsAsync(int categoryId);
}