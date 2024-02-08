namespace ReadersRealm.Services;

using Common.Exceptions;
using Contracts;
using Data.Models;
using Data.Repositories.Contracts;
using ViewModels.ApplicationUser;

public class ApplicationUserService : IApplicationUserService
{
    private readonly IUnitOfWork _unitOfWork;

    public ApplicationUserService(IUnitOfWork unitOfWork)
    {
        this._unitOfWork = unitOfWork;
    }

    public async Task<ApplicationUserViewModel> GetByIdAsync(string id)
    {
        ApplicationUser? applicationUser = await this
            ._unitOfWork
            .ApplicationUserRepository
            .GetByIdAsync(id);

        if (applicationUser == null)
        {
            throw new ApplicationUserNotFoundException();
        }

        ApplicationUserViewModel applicationUserModel = new ApplicationUserViewModel()
        {
            Id = applicationUser.Id,
            FirstName = applicationUser.FirstName,
            LastName = applicationUser.LastName,
        };

        return applicationUserModel;
    }

    public async Task<OrderApplicationUserViewModel> GetApplicationUserForOrderAsync(string id)
    {
        ApplicationUser? applicationUser = await this
            ._unitOfWork
            .ApplicationUserRepository
            .GetByIdAsync(id);

        if (applicationUser == null)
        {
            throw new ApplicationUserNotFoundException();
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
        };

        return applicationUserModel;
    }

    public async Task UpdateApplicationUserAsync(OrderApplicationUserViewModel applicationUserModel)
    {
        ApplicationUser? applicationUser = await this
            ._unitOfWork
            .ApplicationUserRepository
            .GetByIdAsync(applicationUserModel.Id);

        if (applicationUser == null)
        {
            throw new ApplicationUserNotFoundException();
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