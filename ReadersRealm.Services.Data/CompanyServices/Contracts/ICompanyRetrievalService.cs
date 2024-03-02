using ReadersRealm.Common;

namespace ReadersRealm.Services.Data.CompanyServices.Contracts;

using Web.ViewModels.Company;

public interface ICompanyRetrievalService
{
    Task<PaginatedList<AllCompaniesViewModel>> GetAllAsync(int pageIndex, int pageSize, string? searchTerm);
    Task<List<AllCompaniesListViewModel>> GetAllListAsync();
    CreateCompanyViewModel GetCompanyForCreate();
    Task<EditCompanyViewModel> GetCompanyForEditAsync(Guid companyId);
    Task<DeleteCompanyViewModel> GetCompanyForDeleteAsync(Guid companyId);
    Task<bool> CompanyExistsAsync(Guid companyId);
}