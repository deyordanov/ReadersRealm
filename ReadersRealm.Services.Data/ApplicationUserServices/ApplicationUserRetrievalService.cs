namespace ReadersRealm.Services.Data.ApplicationUserServices;

using System.Linq.Expressions;
using Common;
using Common.Exceptions.User;
using Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ReadersRealm.Common.Exceptions.ApplicationUser;
using ReadersRealm.Data.Models;
using ReadersRealm.Data.Repositories.Contracts;
using Web.ViewModels.ApplicationUser;
using static Common.Constants.ExceptionMessages.ApplicationUserExceptionMessages;

public class ApplicationUserRetrievalService : IApplicationUserRetrievalService
{
    private const string PropertiesToInclude = "Company";

    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly IUnitOfWork _unitOfWork;

    public ApplicationUserRetrievalService(
        IUnitOfWork unitOfWork,
        UserManager<ApplicationUser> userManager, 
        RoleManager<IdentityRole<Guid>> roleManager)
    {
        this._unitOfWork = unitOfWork;
        this._userManager = userManager;
        this._roleManager = roleManager;
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
            });
        }

        return PaginatedList<AllApplicationUsersViewModel>.Create(allApplicationUserModels, pageIndex, pageSize);
    }

    public async Task<RolesApplicationUserViewModel> GetApplicationUserForRolesManagementAsync(Guid applicationUserId)
    {
        ApplicationUser? applicationUser = await this
            ._unitOfWork
            .ApplicationUserRepository
            .GetByIdAsync(applicationUserId);

        if (applicationUser == null)
        {
            throw new ApplicationUserNotFoundException(string.Format(ApplicationUserNotFoundExceptionMessage, applicationUserId, nameof(this.GetApplicationUserForRolesManagementAsync)));
        }

        return new RolesApplicationUserViewModel()
        {
            Id = applicationUser.Id,
            FirstName = applicationUser.FirstName,
            LastName = applicationUser.LastName,
            OldRoles = await this.GetUserRoles(applicationUser),
            AllRoles = await this.GetAllRoles(),
        };
    }

    private async Task<IList<string>> GetUserRoles(ApplicationUser applicationUser)
    {
        return await this._userManager.GetRolesAsync(applicationUser);
    }

    private async Task<IList<string>> GetAllRoles()
    {
        return (await this._roleManager.Roles.Select(r => r.Name).ToListAsync())!;
    }
 }