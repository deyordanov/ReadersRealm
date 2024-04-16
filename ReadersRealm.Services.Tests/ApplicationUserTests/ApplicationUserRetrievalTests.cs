namespace ReadersRealm.Services.Tests.ApplicationUserTests;

using System.Linq.Expressions;
using Common;
using Common.Exceptions.ApplicationUser;
using Data.ApplicationUserServices;
using Data.CompanyServices.Contracts;
using Microsoft.AspNetCore.Identity;
using Moq;
using ReadersRealm.Data.Models;
using ReadersRealm.Data.Repositories.Contracts;
using Web.ViewModels.ApplicationUser;
using Web.ViewModels.Company;

[TestFixture]
public class ApplicationUserRetrievalTests
{
    private Mock<IUnitOfWork>? _mockUnitOfWork;
    private Mock<UserManager<ApplicationUser>>? _mockUserManager;
    private Mock<RoleManager<IdentityRole<Guid>>>? _mockRoleManager;
    private Mock<ICompanyRetrievalService>? _mockCompanyRetrievalService;

    private ApplicationUser? _existingUser;
    private List<ApplicationUser>? _users;
    private List<AllCompaniesListViewModel>? _companies;

    [SetUp]
    public void SetUp()
    {
        this._mockUnitOfWork = new Mock<IUnitOfWork>();
        this._mockUserManager = new Mock<UserManager<ApplicationUser>>(
            Mock.Of<IUserStore<ApplicationUser>>(), null!, null!, null!, null!, null!, null!, null!, null!);
        this._mockRoleManager = new Mock<RoleManager<IdentityRole<Guid>>>(
            Mock.Of<IRoleStore<IdentityRole<Guid>>>(), null!, null!, null!, null!);
        this._mockCompanyRetrievalService = new Mock<ICompanyRetrievalService>();

        this._existingUser = new ApplicationUser()
        {
            Id = Guid.NewGuid(),
            FirstName = "FirstName",
            LastName = "LastName",
        };

        this._users = new List<ApplicationUser>() 
            {
            new ApplicationUser()
            {
                Id = Guid.NewGuid(),
                FirstName = "FirstName1",
                LastName = "LastName1",
            },
            new ApplicationUser()
            {
                Id = Guid.NewGuid(),
                FirstName = "FirstName2",
                LastName = "LastName2",
            }
        };

        this._companies = new List<AllCompaniesListViewModel>()
        {
            new AllCompaniesListViewModel()
            {
                Name = "Company1",
            },
            new AllCompaniesListViewModel()
            {
                Name = "Company2",
            },
            new AllCompaniesListViewModel()
            {
                Name = "Company3",
            }
        };

        this._mockUnitOfWork.Setup(uow => uow
                .ApplicationUserRepository
                .GetByIdAsync(this._existingUser.Id))
            .ReturnsAsync(this._existingUser);

        this._mockUnitOfWork.Setup(uow => uow
                .ApplicationUserRepository
                .GetAsync(
                    It.IsAny<Expression<Func<ApplicationUser, bool>>>(),
                    It.IsAny<Func<IQueryable<ApplicationUser>,
                        IOrderedQueryable<ApplicationUser>>>(),
                    It.IsAny<string>()
                ))
            .ReturnsAsync(this._users);

        this._mockCompanyRetrievalService.Setup(service => service
                .GetAllListAsync())
            .ReturnsAsync(this._companies);
    }

    [Test]
    public async Task GetApplicationUserForOrderAsync_ShouldReturnCorrectUser()
    {
        //Arrange
        ApplicationUserRetrievalService service =
            new ApplicationUserRetrievalService(
                this._mockUnitOfWork!.Object,
                this._mockUserManager!.Object,
                this._mockRoleManager!.Object,
                this._mockCompanyRetrievalService!.Object
                );

        //Act
        OrderApplicationUserViewModel applicationUserModel
            = await service.GetApplicationUserForOrderAsync(this._existingUser!.Id);

        //Assert
        Assert.That(this._existingUser.Id, Is.EqualTo(applicationUserModel.Id));
        Assert.That(this._existingUser.FirstName, Is.EqualTo(applicationUserModel.FirstName));
        Assert.That(this._existingUser.LastName, Is.EqualTo(applicationUserModel.LastName));
    }

    [Test]
    public void GetApplicationUserForOrderAsync_ShouldThrowApplicationUserNotFoundException()
    {
        //Arrange
        ApplicationUserRetrievalService service =
            new ApplicationUserRetrievalService(
                this._mockUnitOfWork!.Object,
                this._mockUserManager!.Object,
                this._mockRoleManager!.Object,
                this._mockCompanyRetrievalService!.Object
            );

        //Act
        Assert.ThrowsAsync<ApplicationUserNotFoundException>(async () =>
            await service.GetApplicationUserForOrderAsync(Guid.NewGuid()));
    }

    [Test]
    public async Task GetAllAsync_ShouldReturnAllUsers()
    {
        //Arrange
        ApplicationUserRetrievalService service =
            new ApplicationUserRetrievalService(
                this._mockUnitOfWork!.Object,
                this._mockUserManager!.Object,
                this._mockRoleManager!.Object,
                this._mockCompanyRetrievalService!.Object
            );

        int pageIndex = 0;
        int pageSize = 2;
        string searchTerm = string.Empty;

        //Act
        PaginatedList<AllApplicationUsersViewModel> users 
            = await service.GetAllAsync(pageIndex, pageSize, searchTerm);

        //Assert
        Assert.That(users.Count, Is.EqualTo(pageSize));
        Assert.That(users.Count, Is.EqualTo(this._users!.Count));

        for (int i = 0; i < pageSize; i++)
        {
            Assert.That(users[i].FirstName, Is.EqualTo(this._users[i].FirstName));
        }
    }
}