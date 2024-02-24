namespace ReadersRealm.Services.Data.ApplicationUserServices;

using System.Linq.Expressions;
using Common;
using Common.Exceptions.User;
using Contracts;
using Microsoft.AspNetCore.Identity;
using ReadersRealm.Data.Models;
using ReadersRealm.Data.Repositories.Contracts;
using Web.ViewModels.ApplicationUser;

public class ApplicationUserRetrievalService : IApplicationUserRetrievalService
{
    private const string PropertiesToInclude = "Company";

    // private readonly RoleManager<ApplicationUser> _roleManager;
    private readonly IUnitOfWork _unitOfWork;

    public ApplicationUserRetrievalService(
        IUnitOfWork unitOfWork)
    {
        this._unitOfWork = unitOfWork;
        // this._roleManager = roleManager;
    }

    public async Task<OrderApplicationUserViewModel> GetApplicationUserForOrderAsync(Guid applicationUserId)
    {
        ApplicationUser? applicationUser = await this
            ._unitOfWork
            .ApplicationUserRepository
            .GetByIdAsync(applicationUserId);

        if (applicationUser == null)
        {
            throw new UserNotFoundException();
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
            Email = applicationUser.Email,
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

        List <ApplicationUser> allApplicationUsers = await this
            ._unitOfWork
            .ApplicationUserRepository
            .GetAsync(filter,
                null,
                PropertiesToInclude);

        List<AllApplicationUsersViewModel> allApplicationUserModels = new List<AllApplicationUsersViewModel>();
        //
        // foreach (ApplicationUser applicationUser in allApplicationUsers)
        // {
        //     allApplicationUserModels.Add(new AllApplicationUsersViewModel
        //     {
        //         Id = applicationUser.Id,
        //         FirstName = applicationUser.FirstName,
        //         LastName = applicationUser.LastName,
        //         Email = applicationUser.Email!,
        //         PhoneNumber = applicationUser.PhoneNumber!,
        //         Company = applicationUser.Company,
        //         CompanyId = applicationUser.CompanyId,
        //         Role = (await this._roleManager.GetRoleNameAsync(applicationUser))!
        //     });
        // }

        return PaginatedList<AllApplicationUsersViewModel>.Create(allApplicationUserModels, pageIndex, pageSize);
    }
}