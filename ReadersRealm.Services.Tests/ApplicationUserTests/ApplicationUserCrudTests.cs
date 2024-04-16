namespace ReadersRealm.Services.Tests.ApplicationUser;

using Common.Exceptions.ApplicationUser;
using Microsoft.AspNetCore.Identity;
using Moq;
using ReadersRealm.Data.Models;
using ReadersRealm.Data.Repositories.Contracts;
using Data.ApplicationUserServices;
using ReadersRealm.Web.ViewModels.ApplicationUser;

[TestFixture]
public class ApplicationUserCrudTests
{
    private Mock<IUnitOfWork>? _mockUnitOfWork;
    private Mock<UserManager<ApplicationUser>>? _mockUserManager;
    private Mock<RoleManager<IdentityRole<Guid>>>? _mockRoleManager;
    private ApplicationUser? _existingUser;

    [SetUp]
    public void SetUp()
    {
        this._mockUnitOfWork = new Mock<IUnitOfWork>();
        this._mockUserManager = new Mock<UserManager<ApplicationUser>>(
            Mock.Of<IUserStore<ApplicationUser>>(), null!, null!, null!, null!, null!, null!, null!, null!);
        this._mockRoleManager = new Mock<RoleManager<IdentityRole<Guid>>>(
            Mock.Of<IRoleStore<IdentityRole<Guid>>>(), null!, null!, null!, null!);

        this._existingUser = new ApplicationUser
        {
            Id = Guid.NewGuid(),
            FirstName = "FirstName",
            LastName = "LastName",
        };

        this._mockUnitOfWork.Setup(uow => uow
                .ApplicationUserRepository
                .GetByIdAsync(this._existingUser.Id))
            .ReturnsAsync(this._existingUser);

        this._mockUnitOfWork.Setup(uow => uow
            .SaveAsync())
            .Returns(Task.CompletedTask);

        this._mockUserManager.Setup(um => um
                .GetRolesAsync(It.IsAny<ApplicationUser>()))
            .ReturnsAsync(new List<string> { "OldRole1", "OldRole2" });

        this._mockUserManager.Setup(um => um
                .RemoveFromRoleAsync(It.IsAny<ApplicationUser>(), 
                    It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);
        this._mockUserManager.Setup(um => um
                .AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);
    }

    [Test]
    public async Task UpdateApplicationUserAsync_UpdatesUserCorrectly()
    {
        //Arrange
        ApplicationUserCrudService service = 
            new ApplicationUserCrudService(this._mockUnitOfWork!.Object, this._mockUserManager!.Object, this._mockRoleManager!.Object);

        OrderApplicationUserViewModel applicationUserModel = new OrderApplicationUserViewModel
        {
            Id = this._existingUser!.Id,
            FirstName = "UpdatedFirstName",
            LastName = "UpdatedLastName",
        };

        // Act
        await service.UpdateApplicationUserAsync(applicationUserModel);

        // Assert
        this._mockUnitOfWork.Verify(uow => uow.SaveAsync(), Times.Once); 

        Assert.That(applicationUserModel.FirstName, Is.EqualTo(this._existingUser.FirstName));
        Assert.That(applicationUserModel.LastName, Is.EqualTo(this._existingUser.LastName));
    }

    [Test]
    public void UpdateApplicationUserAsync_ShouldThrowApplicationUserNotFoundException()
    {
        //Arrange
        ApplicationUserCrudService service =
            new ApplicationUserCrudService(this._mockUnitOfWork!.Object, this._mockUserManager!.Object,
                this._mockRoleManager!.Object);

        OrderApplicationUserViewModel applicationUserModel = new OrderApplicationUserViewModel
        {
            Id = Guid.NewGuid(),
            FirstName = "UpdatedFirstName",
            LastName = "UpdatedLastName",
        };

        // Act & Assert
        Assert.ThrowsAsync<ApplicationUserNotFoundException>(async () =>
            await service.UpdateApplicationUserAsync(applicationUserModel));
    }

    [Test]
    public async Task UpdateApplicationUserRolesAsync_UpdatesRolesCorrectly()
    {
        ApplicationUserCrudService service = 
            new ApplicationUserCrudService(this._mockUnitOfWork!.Object, this._mockUserManager!.Object, null!);

        RolesApplicationUserViewModel applicationUserModel = new RolesApplicationUserViewModel
        {
            Id = this._existingUser!.Id,
            FirstName = "",
            LastName = "",
            NewRoles = new List<string> { "NewRole1", "NewRole2" },
            CompanyId = Guid.NewGuid(),
        };

        // Act
        await service.UpdateApplicationUserRolesAsync(applicationUserModel);

        // Assert
        this._mockUserManager.Verify(um => um
            .RemoveFromRoleAsync(this._existingUser, It.IsAny<string>()), Times.Exactly(2));
        this._mockUserManager.Verify(um => um
            .AddToRoleAsync(this._existingUser, It.IsAny<string>()), Times.Exactly(applicationUserModel.NewRoles.Count));

        this._mockUnitOfWork.Verify(uow => uow.SaveAsync(), Times.Once);

        Assert.That(applicationUserModel.CompanyId, Is.EqualTo(this._existingUser.CompanyId));
    }

    [Test]
    public void UpdateApplicationUserRolesAsync_ShouldThrowApplicationUserNotFoundException()
    {
        //Arrange
        ApplicationUserCrudService service =
            new ApplicationUserCrudService(this._mockUnitOfWork!.Object, this._mockUserManager!.Object, null!);

        RolesApplicationUserViewModel applicationUserModel = new RolesApplicationUserViewModel
        {
            Id = Guid.NewGuid(),
            FirstName = "",
            LastName = "",
            NewRoles = new List<string> { "NewRole1", "NewRole2" },
            CompanyId = Guid.NewGuid(),
        };

        //Act & Assert
        Assert.ThrowsAsync<ApplicationUserNotFoundException>(async () =>
            await service.UpdateApplicationUserRolesAsync(applicationUserModel));
    }

    [Test]
    public async Task UpdateApplicationUserLockoutAsync_EnablesLockout()
    {
        //Arrange
        ApplicationUserCrudService service =
            new ApplicationUserCrudService(this._mockUnitOfWork!.Object, this._mockUserManager!.Object, null!);

        //Act
        await service.UpdateApplicationUserLockoutAsync(this._existingUser!.Id, true);

        //Assert
        Assert.That(this._existingUser.LockoutEnd, Is.Not.Null);
        Assert.That(this._existingUser.LockoutEnabled, Is.True);
    }

    [Test]
    public async Task UpdateApplicationUserLockoutAsync_DisabledLockout()
    {
        //Arrange
        ApplicationUserCrudService service =
            new ApplicationUserCrudService(this._mockUnitOfWork!.Object, this._mockUserManager!.Object, null!);

        //Act
        await service.UpdateApplicationUserLockoutAsync(this._existingUser!.Id, false);

        //Assert
        Assert.That(this._existingUser.LockoutEnd, Is.Null);
        Assert.That(this._existingUser.LockoutEnabled, Is.False);
    }

    [Test]
    public void UpdateApplicationUserLockoutAsync_ShouldThrowApplicationUserNotFoundException()
    {
        //Arrange
        ApplicationUserCrudService service =
            new ApplicationUserCrudService(this._mockUnitOfWork!.Object, this._mockUserManager!.Object, null!);

        //Act & Assert
        Assert.ThrowsAsync<ApplicationUserNotFoundException>(async () =>
            await service.UpdateApplicationUserLockoutAsync(Guid.NewGuid(), true));
    }
}
