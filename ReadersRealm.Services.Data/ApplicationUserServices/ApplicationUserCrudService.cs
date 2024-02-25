namespace ReadersRealm.Services.Data.ApplicationUserServices;

using Contracts;
using Microsoft.AspNetCore.Identity;
using ReadersRealm.Common.Exceptions.ApplicationUser;
using ReadersRealm.Data.Models;
using ReadersRealm.Data.Repositories.Contracts;
using Web.ViewModels.ApplicationUser;
using static Common.Constants.ExceptionMessages.ApplicationUserExceptionMessages;

public class ApplicationUserCrudService : IApplicationUserCrudService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly IUnitOfWork _unitOfWork;

    public ApplicationUserCrudService(
        IUnitOfWork unitOfWork, 
        UserManager<ApplicationUser> userManager, 
        RoleManager<IdentityRole<Guid>> roleManager)
    {
        this._unitOfWork = unitOfWork;
        this._userManager = userManager;
        this._roleManager = roleManager;
    }

    public async Task UpdateApplicationUserAsync(OrderApplicationUserViewModel applicationUserModel)
    {
        ApplicationUser? applicationUser = await this
            ._unitOfWork
            .ApplicationUserRepository
            .GetByIdAsync(applicationUserModel.Id);

        if (applicationUser == null)
        {
            throw new ApplicationUserNotFoundException(string.Format(ApplicationUserNotFoundExceptionMessage, applicationUserModel.Id, nameof(this.UpdateApplicationUserAsync)));
        }

        applicationUser.FirstName = applicationUserModel.FirstName;
        applicationUser.LastName = applicationUserModel.LastName;
        applicationUser.City = applicationUserModel.City;
        applicationUser.PostalCode = applicationUserModel.PostalCode;
        applicationUser.State = applicationUserModel.State;
        applicationUser.StreetAddress = applicationUserModel.StreetAddress;
        applicationUser.PhoneNumber = applicationUserModel.PhoneNumber;

        await this
            ._unitOfWork
            .SaveAsync();
    }

    public async Task UpdateApplicationUserRolesAsync(RolesApplicationUserViewModel applicationUserModel)
    {
        ApplicationUser? applicationUser = await this
            ._unitOfWork
            .ApplicationUserRepository
            .GetByIdAsync(applicationUserModel.Id);

        if (applicationUser == null)
        {
            throw new ApplicationUserNotFoundException(string.Format(ApplicationUserNotFoundExceptionMessage, applicationUserModel.Id, nameof(this.UpdateApplicationUserRolesAsync)));
        }

        IList<string> oldRoles = await this.GetUserRoles(applicationUser);

        foreach (string oldRole in oldRoles)
        {
            await _userManager.RemoveFromRoleAsync(applicationUser, oldRole);
        }

        foreach (string newRole in applicationUserModel.NewRoles)
        {
            await _userManager.AddToRoleAsync(applicationUser, newRole);
        }

        if (applicationUserModel.CompanyId != null)
        {
            applicationUser.CompanyId = applicationUserModel.CompanyId;

            await this
                ._unitOfWork
                .SaveAsync();
        }
    }

    public async Task UpdateApplicationUserLockoutAsync(Guid applicationUserId, bool status)
    {
        ApplicationUser? applicationUser = await this
            ._unitOfWork
            .ApplicationUserRepository
            .GetByIdAsync(applicationUserId);

        if (applicationUser == null)
        {
            throw new ApplicationUserNotFoundException(string.Format(ApplicationUserNotFoundExceptionMessage, applicationUserId, nameof(this.UpdateApplicationUserLockoutAsync)));
        }

        if (status)
        {
            applicationUser.LockoutEnd = DateTime.UtcNow.AddYears(3);
        }
        else
        {
            applicationUser.LockoutEnd = null;
        }

        await this
            ._unitOfWork
            .SaveAsync();
    }

    private async Task<IList<string>> GetUserRoles(ApplicationUser applicationUser)
    {
        return await this._userManager.GetRolesAsync(applicationUser);
    }
}