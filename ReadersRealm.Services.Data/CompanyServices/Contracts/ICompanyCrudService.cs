namespace ReadersRealm.Services.Data.CompanyServices.Contracts;

using Web.ViewModels.Company;

public interface ICompanyCrudService
{
    Task CreateCompanyAsync(CreateCompanyViewModel bookModel);
    Task EditCompanyAsync(EditCompanyViewModel bookModel);
    Task DeleteCompanyAsync(DeleteCompanyViewModel bookModel);
}