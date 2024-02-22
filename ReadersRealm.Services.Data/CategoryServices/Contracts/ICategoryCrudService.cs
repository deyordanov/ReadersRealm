namespace ReadersRealm.Services.Data.CategoryServices.Contracts;

using Web.ViewModels.Category;

public interface ICategoryCrudService
{
    Task CreateCategoryAsync(CreateCategoryViewModel categoryModel);
    Task EditCategoryAsync(EditCategoryViewModel categoryModel);
    Task DeleteCategoryAsync(DeleteCategoryViewModel categoryModel);
}