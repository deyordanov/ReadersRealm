namespace ReadersRealm.Services;

using Contracts;
using ViewModels.ApplicationUser;

public class ApplicationUserService : IApplicationUserService
{
    public Task<ApplicationUserViewModel> GetByIdAsync(string id)
    {
        throw new NotImplementedException();
    }
}