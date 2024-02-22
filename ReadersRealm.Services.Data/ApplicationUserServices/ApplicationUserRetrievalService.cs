namespace ReadersRealm.Services.Data.ApplicationUserServices;

using Common.Exceptions.User;
using Contracts;
using ReadersRealm.Data.Models;
using ReadersRealm.Data.Repositories.Contracts;
using Web.ViewModels.ApplicationUser;

public class ApplicationUserRetrievalService : IApplicationUserRetrievalService
{
    private readonly IUnitOfWork _unitOfWork;

    public ApplicationUserRetrievalService(IUnitOfWork unitOfWork)
    {
        this._unitOfWork = unitOfWork;
    }

    public async Task<OrderApplicationUserViewModel> GetApplicationUserForOrderAsync(string id)
    {
        ApplicationUser? applicationUser = await this
            ._unitOfWork
            .ApplicationUserRepository
            .GetByIdAsync(id);

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
}