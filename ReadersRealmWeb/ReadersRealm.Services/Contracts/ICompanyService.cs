namespace ReadersRealm.Services.Contracts;

using Data.Models;
using Common;
using ViewModels.Company;
using ReadersRealm.ViewModels.Category;

public interface ICompanyService
{
    Task<PaginatedList<AllCompaniesViewModel>> GetAllAsync(int pageIndex, int pageSize, string? searchTerm);
    Task<Company?> GetCompanyByIdAsync(Guid id);
    Task<List<AllCompaniesListViewModel>> GetAllListAsync();
    CreateCompanyViewModel GetCompanyForCreate();
    Task<EditCompanyViewModel> GetCompanyForEditAsync(Guid id);
    Task<DeleteCompanyViewModel> GetCompanyForDeleteAsync(Guid id);
    Task CreateCompanyAsync(CreateCompanyViewModel bookModel);
    Task EditCompanyAsync(EditCompanyViewModel bookModel);
    Task DeleteCompanyAsync(DeleteCompanyViewModel bookModel);
}