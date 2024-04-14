namespace ReadersRealm.Services.Tests.IdentityTests;

using Data.IdentityServices;
using Data.IdentityServices.Contracts;
using Web.ViewModels.Identity;

[TestFixture]
public class IdentityRetrievalTests
{
    [Test]
    public void GetLoginModelForLogin_ShouldReturnCorrectLoginModel()
    {
        // Arrange
        IIdentityRetrievalService service = new IdentityRetrievalService();

        // Act
        LoginViewModel loginModel = service.GetLoginModelForLogin();

        // Assert
        Assert.IsNotNull(loginModel);
        Assert.IsNotNull(loginModel.Input);
        Assert.That(loginModel.Input.Email, Is.EqualTo(string.Empty));
        Assert.That(loginModel.Input.Password, Is.EqualTo(string.Empty));
    }

    [Test]
    public void GetRegisterModelForRegister_ShouldReturnCorrectRegisterModel()
    {
        // Arrange
        IIdentityRetrievalService service = new IdentityRetrievalService();

        // Act
        RegisterViewModel registerModel = service.GetRegisterModelForRegister();

        // Assert
        Assert.IsNotNull(registerModel);
        Assert.IsNotNull(registerModel.Input);
        Assert.That(registerModel.Input.Email, Is.EqualTo(string.Empty));
        Assert.That(registerModel.Input.Password, Is.EqualTo(string.Empty));
        Assert.That(registerModel.Input.ConfirmPassword, Is.EqualTo(string.Empty));
        Assert.That(registerModel.Input.FirstName, Is.EqualTo(string.Empty));
        Assert.That(registerModel.Input.LastName, Is.EqualTo(string.Empty));
    }
}