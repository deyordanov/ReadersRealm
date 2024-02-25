namespace ReadersRealm.Services.Data.ApplicationUserServices.Contracts;

using Web.ViewModels.ApplicationUser;

public interface IApplicationUserCrudService
{
    Task UpdateApplicationUserAsync(OrderApplicationUserViewModel applicationUserModel);
    Task UpdateApplicationUserRolesAsync(RolesApplicationUserViewModel applicationUserModel);
    Task UpdateApplicationUserLockoutAsync(Guid applicationUserId, bool status);
}