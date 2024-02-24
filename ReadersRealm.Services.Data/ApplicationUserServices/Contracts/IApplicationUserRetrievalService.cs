namespace ReadersRealm.Services.Data.ApplicationUserServices.Contracts;

using Common;
using Web.ViewModels.ApplicationUser;

public interface IApplicationUserRetrievalService
{
    public Task<OrderApplicationUserViewModel> GetApplicationUserForOrderAsync(Guid applicationUserId);
    public Task<PaginatedList<AllApplicationUsersViewModel>> GetAllAsync(int pageIndex, int pageSize,
        string? searchTerm);
    public Task<RolesApplicationUserViewModel> GetApplicationUserForRolesManagementAsync(Guid applicationUserId);
}