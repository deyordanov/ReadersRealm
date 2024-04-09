namespace ReadersRealm.Services.Tests.CompanyTests;

using Common.Exceptions.Company;
using Data.CompanyServices;
using Data.CompanyServices.Contracts;
using Moq;
using ReadersRealm.Data.Models;
using ReadersRealm.Data.Repositories.Contracts;
using Web.ViewModels.Company;

[TestFixture]
public class CompanyCrudTests
{
    private Mock<IUnitOfWork>? _mockUnitOfWork;

    private Company? _existingCompany;

    [SetUp]
    public void SetUp()
    {
        this._mockUnitOfWork = new Mock<IUnitOfWork>();

        this._existingCompany = new Company()
        {
            Name = "Name",
            Email = "Email",
            UIC = "UIC",
        };

        this._mockUnitOfWork.Setup(uow => uow
                .CompanyRepository
                .AddAsync(It.IsAny<Company>()))
            .Returns(Task.CompletedTask);

        this._mockUnitOfWork.Setup(uow => uow
                .CompanyRepository
                .GetByIdAsync(this._existingCompany!.Id))
            .ReturnsAsync(this._existingCompany);
    }

    [Test]
    public async Task CreateCompanyAsync_ShouldCreateCompany()
    {
        //Arrange
        ICompanyCrudService service
            = new CompanyCrudService(this._mockUnitOfWork!.Object);

        CreateCompanyViewModel companyModel = new CreateCompanyViewModel()
        {
            Name = this._existingCompany!.Name,
            Email = this._existingCompany!.Email,
            UIC = this._existingCompany!.UIC,
        };

        //Act
        await service.CreateCompanyAsync(companyModel);

        //Assert
        this._mockUnitOfWork.Verify(uow => uow
            .CompanyRepository
            .AddAsync(It.Is<Company>(b =>
                b.Name == this._existingCompany!.Name)));

        this._mockUnitOfWork.Verify(uow => uow.SaveAsync(), Times.Once());
    }

    [Test]
    public async Task EditCompanyAsync_ShouldUpdateCompanyCorrectly()
    {
        //Arrange
        ICompanyCrudService service
            = new CompanyCrudService(this._mockUnitOfWork!.Object);

        EditCompanyViewModel companyModel = new EditCompanyViewModel()
        {
            Id = this._existingCompany!.Id,
            Name = "UpdatedName",
            Email = "UpdatedEmail",
            UIC = "UpdatedUIC",
        };

        //Act
        await service.EditCompanyAsync(companyModel);

        //Assert
        this._mockUnitOfWork.Verify(uow => uow.SaveAsync(), Times.Once());

        Assert.That(companyModel.Id, Is.EqualTo(this._existingCompany!.Id));
        Assert.That(companyModel.Name, Is.EqualTo(this._existingCompany!.Name));
    }

    [Test]
    public void EditCompanyAsync_ShouldThrowCompanyNotFoundException()
    {
        //Arrange
        ICompanyCrudService service
            = new CompanyCrudService(this._mockUnitOfWork!.Object);

        EditCompanyViewModel companyModel = new EditCompanyViewModel()
        {
            Id = Guid.NewGuid(),
            Name = "UpdatedName",
            Email = "UpdatedEmail",
            UIC = "UpdatedUIC",
        };

        //Act & Assert
        Assert.ThrowsAsync<CompanyNotFoundException>(async () =>
        await service.EditCompanyAsync(companyModel));
    }

    [Test]
    public async Task DeleteCompanyAsync_ShouldDeleteCompany()
    {
        //Arrange
        ICompanyCrudService service
            = new CompanyCrudService(this._mockUnitOfWork!.Object);

        DeleteCompanyViewModel companyModel = new DeleteCompanyViewModel()
        {
            Id = this._existingCompany!.Id,
            Name = this._existingCompany!.Name,
            Email = this._existingCompany!.Email,
            UIC = this._existingCompany!.UIC,
        };

        //Act
        await service.DeleteCompanyAsync(companyModel);

        //Assert
        this._mockUnitOfWork.Verify(uow => uow
            .CompanyRepository
            .Delete(It.Is<Company>(b =>
                b.Id == this._existingCompany!.Id &&
                b.Name == this._existingCompany!.Name)));

        this._mockUnitOfWork.Verify(uow => uow.SaveAsync(), Times.Once());
    }

    [Test]
    public void DeleteCompanyAsync_ShouldThrowCompanyNotFoundException()
    {
        //Arrange
        ICompanyCrudService service
            = new CompanyCrudService(this._mockUnitOfWork!.Object);

        DeleteCompanyViewModel companyModel = new DeleteCompanyViewModel()
        {
            Id = Guid.NewGuid(),
            Name = this._existingCompany!.Name,
            Email = this._existingCompany!.Email,
            UIC = this._existingCompany!.UIC,
        };

        //Act & Assert
        Assert.ThrowsAsync<CompanyNotFoundException>(async () =>
            await service.DeleteCompanyAsync(companyModel));
    }
}