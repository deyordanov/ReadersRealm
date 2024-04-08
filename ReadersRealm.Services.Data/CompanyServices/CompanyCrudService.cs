namespace ReadersRealm.Services.Data.CompanyServices;

using Common.Exceptions.Company;
using Contracts;
using ReadersRealm.Data.Models;
using ReadersRealm.Data.Repositories.Contracts;
using Web.ViewModels.Company;

public class CompanyCrudService(IUnitOfWork unitOfWork) : ICompanyCrudService
{
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

        await unitOfWork
            .CompanyRepository
            .AddAsync(companyToAdd);

        await unitOfWork
            .SaveAsync();
    }

    public async Task EditCompanyAsync(EditCompanyViewModel bookModel)
    {
        Company? companyToEdit = await unitOfWork
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

        await unitOfWork
            .SaveAsync();
    }

    public async Task DeleteCompanyAsync(DeleteCompanyViewModel companyModel)
    {
        Company? companyToDelete = await unitOfWork
            .CompanyRepository
            .GetByIdAsync(companyModel.Id);

        if (companyToDelete == null)
        {
            throw new CompanyNotFoundException();
        }

        unitOfWork
            .CompanyRepository
            .Delete(companyToDelete);

        await unitOfWork
            .SaveAsync();
    }
}