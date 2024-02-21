namespace ReadersRealm.Services.Data;

using Common.Exceptions.User;
using Contracts;
using ReadersRealm.Data.Models;
using ReadersRealm.Data.Repositories.Contracts;
using ViewModels.ApplicationUser;

public class ApplicationUserService : IApplicationUserService
{
    private readonly IUnitOfWork _unitOfWork;

    public ApplicationUserService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<OrderApplicationUserViewModel> GetApplicationUserForOrderAsync(string id)
    {
        ApplicationUser? applicationUser = await _unitOfWork
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

    public async Task UpdateApplicationUserAsync(OrderApplicationUserViewModel applicationUserModel)
    {
        ApplicationUser? applicationUser = await _unitOfWork
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

        await _unitOfWork
            .SaveAsync();
    }
}