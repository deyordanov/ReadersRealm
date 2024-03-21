namespace ReadersRealm.Services.Data.IdentityServices.Contracts;

using Web.ViewModels.Identity;

public interface IIdentityRetrievalService
{
    public LoginViewModel GetLoginModelForLogin();

    public RegisterViewModel GetRegisterModelForRegister();
}