namespace ReadersRealm.Services.Data.ApplicationUserServices;

using Common.Exceptions.User;
using Contracts;
using ReadersRealm.Data.Models;
using ReadersRealm.Data.Repositories.Contracts;
using Web.ViewModels.ApplicationUser;

public class ApplicationUserCrudService : IApplicationUserCrudService
{
    private readonly IUnitOfWork _unitOfWork;

    public ApplicationUserCrudService(IUnitOfWork unitOfWork)
    {
        this._unitOfWork = unitOfWork;
    }

    public async Task UpdateApplicationUserAsync(OrderApplicationUserViewModel applicationUserModel)
    {
        ApplicationUser? applicationUser = await this
            ._unitOfWork
            .ApplicationUserRepository
            .GetByIdAsync(applicationUserModel.Id);

        if (applicationUser == null)
        {
            throw new UserNotFoundException();
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
}