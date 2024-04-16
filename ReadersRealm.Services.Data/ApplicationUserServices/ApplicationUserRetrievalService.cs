#pragma warning disable IDE0270
namespace ReadersRealm.Services.Data.ApplicationUserServices;

using System.Linq.Expressions;
using Common;
using CompanyServices.Contracts;
using Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Common.Exceptions.ApplicationUser;
using ReadersRealm.Data.Models;
using ReadersRealm.Data.Repositories.Contracts;
using Web.ViewModels.ApplicationUser;
using Web.ViewModels.Company;
using static Common.Constants.ExceptionMessages.ApplicationUserExceptionMessages;

public class ApplicationUserRetrievalService(
    IUnitOfWork unitOfWork,
    UserManager<ApplicationUser> userManager,
    RoleManager<IdentityRole<Guid>> roleManager,
    ICompanyRetrievalService companyRetrievalService)
    : IApplicationUserRetrievalService
{
    private const string PropertiesToInclude = "Company";

    public async Task<OrderApplicationUserViewModel> GetApplicationUserForOrderAsync(Guid applicationUserId)
    {
        ApplicationUser? applicationUser = await unitOfWork
            .ApplicationUserRepository
            .GetByIdAsync(applicationUserId);

        if (applicationUser == null)
        {
            throw new ApplicationUserNotFoundException(string.Format(ApplicationUserNotFoundExceptionMessage, applicationUserId, nameof(this.GetApplicationUserForOrderAsync)));
        }

        OrderApplicationUserViewModel applicationUserModel = new OrderApplicationUserViewModel()
        {
            Id = applicationUser.Id,
            FirstName = applicationUser.FirstName,
            LastName = applicationUser.LastName,
            City = applicationUser.City,
            PostalCode = applicationUser.PostalCode,
            State = applicationUser.State,
            StreetAddress = applicationUser.StreetAddress,
            PhoneNumber = applicationUser.PhoneNumber,
            Email = applicationUser.Email!,
        };

        return applicationUserModel;
    }

    public async Task<PaginatedList<AllApplicationUsersViewModel>> GetAllAsync(int pageIndex, int pageSize, string? searchTerm)
    {
        Expression<Func<ApplicationUser, bool>> filter = applicationUser => 
            applicationUser
                .FirstName
                .ToLower()
                .StartsWith(searchTerm != null ? searchTerm.ToLower() : string.Empty) ||
            applicationUser
                .LastName
                .ToLower()
                .StartsWith(searchTerm != null ? searchTerm.ToLower() : string.Empty);

        List <ApplicationUser> allApplicationUsers = await unitOfWork
            .ApplicationUserRepository
            .GetAsync(filter,
                null,
                PropertiesToInclude);

        List<AllApplicationUsersViewModel> allApplicationUserModels = new List<AllApplicationUsersViewModel>();
        
        foreach (ApplicationUser applicationUser in allApplicationUsers)
        {
            allApplicationUserModels.Add(new AllApplicationUsersViewModel
            {
                Id = applicationUser.Id,
                FirstName = applicationUser.FirstName,
                LastName = applicationUser.LastName,
                Email = applicationUser.Email!,
                PhoneNumber = applicationUser.PhoneNumber!,
                Company = applicationUser.Company,
                CompanyId = applicationUser.CompanyId,
                Roles = await GetUserRoles(applicationUser),
                IsLocked = applicationUser.LockoutEnd != null,
            });
        }

        return PaginatedList<AllApplicationUsersViewModel>.Create(allApplicationUserModels, pageIndex, pageSize);
    }

    public async Task<RolesApplicationUserViewModel> GetApplicationUserForRolesManagementAsync(Guid applicationUserId)
    {
        ApplicationUser? applicationUser = await unitOfWork
            .ApplicationUserRepository
            .GetByIdAsync(applicationUserId);

        if (applicationUser == null)
        {
            throw new ApplicationUserNotFoundException(string.Format(ApplicationUserNotFoundExceptionMessage, applicationUserId, nameof(this.GetApplicationUserForRolesManagementAsync)));
        }

        List<AllCompaniesListViewModel> allCompanies = await companyRetrievalService.GetAllListAsync();

        return new RolesApplicationUserViewModel()
        {
            Id = applicationUser.Id,
            FirstName = applicationUser.FirstName,
            LastName = applicationUser.LastName,
            OldRoles = await this.GetUserRoles(applicationUser),
            AllRoles = await this.GetAllRoles(),
            Companies = allCompanies
                .Select(company => 
                    new SelectListItem(company.Name, company
                    .Id
                    .ToString()))
                .ToList(),
            CompanyId = applicationUser.CompanyId,
        };
    }

    private async Task<IList<string>> GetUserRoles(ApplicationUser applicationUser)
    {
        return await userManager.GetRolesAsync(applicationUser);
    }

    private async Task<IList<string>> GetAllRoles()
    {
        return (await roleManager.Roles.Select(r => r.Name).ToListAsync())!;
    }
 }