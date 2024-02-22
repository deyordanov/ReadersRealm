namespace ReadersRealm.Services.Data.ApplicationUserServices.Contracts;

using Web.ViewModels.ApplicationUser;

public interface IApplicationUserRetrievalService
{
    public Task<OrderApplicationUserViewModel> GetApplicationUserForOrderAsync(string applicationUserId);
}