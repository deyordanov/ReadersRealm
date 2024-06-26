﻿namespace ReadersRealm.Services.Data.CompanyServices;

using Common;
using Common.Exceptions.Company;
using Contracts;
using ReadersRealm.Data.Models;
using ReadersRealm.Data.Repositories.Contracts;
using Web.ViewModels.Company;

public class CompanyRetrievalService(IUnitOfWork unitOfWork) : ICompanyRetrievalService
{
    public async Task<PaginatedList<AllCompaniesViewModel>> GetAllAsync(int pageIndex, int pageSize, string? searchTerm)
    {
        IEnumerable<Company> allCompanies = await unitOfWork
            .CompanyRepository
            .GetAsync(company => company.Name.ToLower().StartsWith(searchTerm != null ? searchTerm.ToLower() : string.Empty), null, string.Empty);

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

    public async Task<List<AllCompaniesListViewModel>> GetAllListAsync()
    {
        IEnumerable<Company> allCompanies = await unitOfWork
            .CompanyRepository
            .GetAsync(null, null, string.Empty);

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
            Name = string.Empty,
            UIC = string.Empty,
            Email = string.Empty,
        };
    }

    public async Task<EditCompanyViewModel> GetCompanyForEditAsync(Guid companyId)
    {
        Company? company = await unitOfWork
            .CompanyRepository
            .GetByIdAsync(companyId);

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

    public async Task<DeleteCompanyViewModel> GetCompanyForDeleteAsync(Guid companyId)
    {
        Company? company = await unitOfWork
            .CompanyRepository
            .GetByIdAsync(companyId);

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

    public async Task<bool> CompanyExistsAsync(Guid companyId)
    {
        return await unitOfWork
            .CompanyRepository
            .GetFirstOrDefaultByFilterAsync(company => company.Id.Equals(companyId), false) != null;
    }
}