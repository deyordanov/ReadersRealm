namespace ReadersRealm.Services;

using Common;
using Common.Exceptions;
using Contracts;
using Data.Models;
using Data.Repositories.Contracts;
using ViewModels.Company;

public class CompanyService : ICompanyService
{
    private readonly IUnitOfWork unitOfWork;

    public CompanyService(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    public async Task<PaginatedList<AllCompaniesViewModel>> GetAllAsync(int pageIndex, int pageSize, string? searchTerm)
    {
        IEnumerable<Company> allCompanies = await this
            .unitOfWork
            .CompanyRepository
            .GetAsync(company => company.Name.ToLower().StartsWith(searchTerm != null ? searchTerm.ToLower() : ""), null, "");

        return PaginatedList<AllCompaniesViewModel>.Create(allCompanies.Select(c => new AllCompaniesViewModel()
        {
            Id = c.Id,
            Name = c.Name,
            UIC = c.UIC,
            Email = c.Email,
            StreetAddress = c.StreetAddress,
            City = c.City,
            PostalCode = c.PostalCode,
            State = c.State,
            PhoneNumber = c.PhoneNumber,
        })
            .ToList(), pageIndex, pageSize);
    }

    public async Task<Company?> GetCompanyByIdAsync(Guid id)
    {
        return await this
            .unitOfWork
            .CompanyRepository
            .GetByIdAsync(id);
    }

    public async Task<List<AllCompaniesListViewModel>> GetAllListAsync()
    {
        IEnumerable<Company> allCompanies = await this
            .unitOfWork
            .CompanyRepository
            .GetAsync(null, null, "");

        List<AllCompaniesListViewModel> companiesToReturn = allCompanies
            .Select(company => new AllCompaniesListViewModel()
        {
            Id = company.Id,
            Name = company.Name,
        }).ToList();

        return companiesToReturn;
    }

    public CreateCompanyViewModel GetCompanyForCreate()
    {
        return new CreateCompanyViewModel()
        {
            Name = "",
            UIC = "",
            Email = "",
        };
    }

    public async Task<EditCompanyViewModel> GetCompanyForEditAsync(Guid id)
    {
        Company? company = await this
            .unitOfWork
            .CompanyRepository
            .GetByIdAsync(id);

        if (company == null)
        {
            throw new CompanyNotFoundException();
        }

        EditCompanyViewModel companyModel = new EditCompanyViewModel()
        {
            Id = company.Id,
            Name = company.Name,
            UIC = company.UIC,
            Email = company.Email,
            StreetAddress = company.StreetAddress,
            City = company.City,
            PostalCode = company.PostalCode,
            State = company.State,
            PhoneNumber = company.PhoneNumber,
        };

        return companyModel;
    }

    public async Task<DeleteCompanyViewModel> GetCompanyForDeleteAsync(Guid id)
    {
        Company? company = await this
            .unitOfWork
            .CompanyRepository
            .GetByIdAsync(id);

        if (company == null)
        {
            throw new CompanyNotFoundException();
        }

        DeleteCompanyViewModel companyModel = new DeleteCompanyViewModel()
        {
            Id = company.Id,
            Name = company.Name,
            UIC = company.UIC,
            Email = company.Email,
            StreetAddress = company.StreetAddress,
            City = company.City,
            PostalCode = company.PostalCode,
            State = company.State,
            PhoneNumber = company.PhoneNumber,
        };

        return companyModel;
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
            .unitOfWork
            .CompanyRepository
            .AddAsync(companyToAdd);

        await this
            .unitOfWork
            .SaveAsync();
    }

    public async Task EditCompanyAsync(EditCompanyViewModel bookModel)
    {
        Company? companyToEdit = await this
            .unitOfWork
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

        await this.unitOfWork.SaveAsync();
    }

    public async Task DeleteCompanyAsync(DeleteCompanyViewModel companyModel)
    {
        Company? companyToDelete = await this
            .unitOfWork
            .CompanyRepository
            .GetByIdAsync(companyModel.Id);

        if (companyToDelete == null)
        {
            throw new CompanyNotFoundException();
        }

        this
            .unitOfWork
            .CompanyRepository
            .Delete(companyToDelete);

        await this 
            .unitOfWork
            .SaveAsync();
    }
}