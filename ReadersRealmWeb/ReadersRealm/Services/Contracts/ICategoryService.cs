namespace ReadersRealm.Services.Contracts;

using ViewModels.Category;

public interface ICategoryService
{
    Task<IEnumerable<AllCategoriesViewModel>> GetAllAsync();

    Task CreateCategoryAsync(CreateCategoryViewModel categoryModel);
}