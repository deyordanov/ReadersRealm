namespace ReadersRealm.Services.Data.Contracts;

using ReadersRealm.Common;
using ReadersRealm.ViewModels.Company;

public interface ICompanyService
{
    Task<PaginatedList<AllCompaniesViewModel>> GetAllAsync(int pageIndex, int pageSize, string? searchTerm);
    Task<List<AllCompaniesListViewModel>> GetAllListAsync();
    CreateCompanyViewModel GetCompanyForCreate();
    Task<EditCompanyViewModel> GetCompanyForEditAsync(Guid id);
    Task<DeleteCompanyViewModel> GetCompanyForDeleteAsync(Guid id);
    Task CreateCompanyAsync(CreateCompanyViewModel bookModel);
    Task EditCompanyAsync(EditCompanyViewModel bookModel);
    Task DeleteCompanyAsync(DeleteCompanyViewModel bookModel);
}