namespace ReadersRealm.Services.Data.CompanyServices;

using Common.Exceptions.Company;
using Contracts;
using ReadersRealm.Data.Models;
using ReadersRealm.Data.Repositories.Contracts;
using Web.ViewModels.Company;

public class CompanyCrudService : ICompanyCrudService
{
    private readonly IUnitOfWork _unitOfWork;

    public CompanyCrudService(IUnitOfWork unitOfWork)
    {
        this._unitOfWork = unitOfWork;
    }

    public async Task CreateCompanyAsync(CreateCompanyViewModel bookModel)
    {
        Company companyToAdd = new Company()
        {
            Name = bookModel.Name,
            UIC = bookModel.UIC,
            Email = bookModel.Email,
            StreetAddress = bookModel.StreetAddress,
            City = bookModel.City,
            PostalCode = bookModel.PostalCode,
            State = bookModel.State,
            PhoneNumber = bookModel.PhoneNumber,
        };

        await this
            ._unitOfWork
            .CompanyRepository
            .AddAsync(companyToAdd);

        await this
            ._unitOfWork
            .SaveAsync();
    }

    public async Task EditCompanyAsync(EditCompanyViewModel bookModel)
    {
        Company? companyToEdit = await this
            ._unitOfWork
            .CompanyRepository
            .GetByIdAsync(bookModel.Id);

        if (companyToEdit == null)
        {
            throw new CompanyNotFoundException();
        }

        companyToEdit.Id = bookModel.Id;
        companyToEdit.Name = bookModel.Name;
        companyToEdit.UIC = bookModel.UIC;
        companyToEdit.Email = bookModel.Email;
        companyToEdit.StreetAddress = bookModel.StreetAddress;
        companyToEdit.City = bookModel.City;
        companyToEdit.PostalCode = bookModel.PostalCode;
        companyToEdit.State = bookModel.State;
        companyToEdit.PhoneNumber = bookModel.PhoneNumber;

        await this
            ._unitOfWork
            .SaveAsync();
    }

    public async Task DeleteCompanyAsync(DeleteCompanyViewModel companyModel)
    {
        Company? companyToDelete = await this
            ._unitOfWork
            .CompanyRepository
            .GetByIdAsync(companyModel.Id);

        if (companyToDelete == null)
        {
            throw new CompanyNotFoundException();
        }

        this._unitOfWork
            .CompanyRepository
            .Delete(companyToDelete);

        await this
            ._unitOfWork
            .SaveAsync();
    }
}