namespace ReadersRealm.Services.Data.IdentityServices;

using Contracts;
using Web.ViewModels.Identity;

public class IdentityRetrievalService : IIdentityRetrievalService
{
    public LoginViewModel GetLoginModelForLogin()
    {
        return new LoginViewModel()
        {
            Input = new LoginInputViewModel()
            {
                Email = string.Empty,
                Password = string.Empty,
            },
        };
    }

    public RegisterViewModel GetRegisterModelForRegister()
    {
        return new RegisterViewModel()
        {
            Input = new RegisterInputViewModel()
            {
                Email = string.Empty,
                Password = string.Empty,
                ConfirmPassword = string.Empty,
                FirstName = string.Empty,
                LastName = string.Empty,
            }
        };
    }
}