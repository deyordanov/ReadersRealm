namespace ReadersRealm.Services.Data.ApplicationUserServices;

using Contracts;
using Microsoft.AspNetCore.Identity;
using Common.Exceptions.ApplicationUser;
using ReadersRealm.Data.Models;
using ReadersRealm.Data.Repositories.Contracts;
using Web.ViewModels.ApplicationUser;
using static Common.Constants.ExceptionMessages.ApplicationUserExceptionMessages;

public class ApplicationUserCrudService(
    IUnitOfWork unitOfWork,
    UserManager<ApplicationUser> userManager,
    RoleManager<IdentityRole<Guid>> roleManager)
    : IApplicationUserCrudService
{
    private readonly RoleManager<IdentityRole<Guid>> _roleManager = roleManager;

    public async Task UpdateApplicationUserAsync(OrderApplicationUserViewModel applicationUserModel)
    {
        ApplicationUser? applicationUser = await unitOfWork
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

        await unitOfWork
            .SaveAsync();
    }

    public async Task UpdateApplicationUserRolesAsync(RolesApplicationUserViewModel applicationUserModel)
    {
        ApplicationUser? applicationUser = await unitOfWork
            .ApplicationUserRepository
            .GetByIdAsync(applicationUserModel.Id);

        if (applicationUser == null)
        {
            throw new ApplicationUserNotFoundException(string.Format(ApplicationUserNotFoundExceptionMessage, applicationUserModel.Id, nameof(this.UpdateApplicationUserRolesAsync)));
        }

        IList<string> oldRoles = await this.GetUserRoles(applicationUser);

        foreach (string oldRole in oldRoles)
        {
            await userManager.RemoveFromRoleAsync(applicationUser, oldRole);
        }

        foreach (string newRole in applicationUserModel.NewRoles)
        {
            await userManager.AddToRoleAsync(applicationUser, newRole);
        }

        if (applicationUserModel.CompanyId != null)
        {
            applicationUser.CompanyId = applicationUserModel.CompanyId;

            await unitOfWork
                .SaveAsync();
        }
    }

    public async Task UpdateApplicationUserLockoutAsync(Guid applicationUserId, bool status)
    {
        ApplicationUser? applicationUser = await unitOfWork
            .ApplicationUserRepository
            .GetByIdAsync(applicationUserId);

        if (applicationUser == null)
        {
            throw new ApplicationUserNotFoundException(string.Format(ApplicationUserNotFoundExceptionMessage, applicationUserId, nameof(this.UpdateApplicationUserLockoutAsync)));
        }

        if (status)
        {
            applicationUser.LockoutEnd = DateTime.UtcNow.AddYears(3);
            applicationUser.LockoutEnabled = true;
        }
        else
        {
            applicationUser.LockoutEnd = null;
            applicationUser.LockoutEnabled = false;
        }

        await unitOfWork
            .SaveAsync();
    }

    private async Task<IList<string>> GetUserRoles(ApplicationUser applicationUser)
    {
        return await userManager.GetRolesAsync(applicationUser);
    }
}