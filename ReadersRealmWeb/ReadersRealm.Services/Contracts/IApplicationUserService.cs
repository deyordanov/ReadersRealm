namespace ReadersRealm.Services.Contracts;

using ViewModels.ApplicationUser;

public interface IApplicationUserService
{
    public Task<ApplicationUserViewModel> GetByIdAsync(string id);
}