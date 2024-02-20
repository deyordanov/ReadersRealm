namespace ReadersRealm.Services.Data.Contracts;

using ReadersRealm.ViewModels.ApplicationUser;

public interface IApplicationUserService
{
    Task<OrderApplicationUserViewModel> GetApplicationUserForOrderAsync(string id);
    Task UpdateApplicationUserAsync(OrderApplicationUserViewModel applicationUserModel);
}