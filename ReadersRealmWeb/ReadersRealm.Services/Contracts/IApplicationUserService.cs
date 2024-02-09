namespace ReadersRealm.Services.Contracts;

using ViewModels.ApplicationUser;

public interface IApplicationUserService
{
    Task<ApplicationUserViewModel> GetByIdAsync(string id);
    Task<OrderApplicationUserViewModel> GetApplicationUserForOrderAsync(string id);
    Task UpdateApplicationUserAsync(OrderApplicationUserViewModel applicationUserModel);
}