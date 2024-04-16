namespace ReadersRealm.Services.Tests.CompanyTests;

using System.Linq.Expressions;
using Common;
using Common.Exceptions.Company;
using Data.CompanyServices;
using Data.CompanyServices.Contracts;
using Moq;
using ReadersRealm.Data.Models;
using ReadersRealm.Data.Repositories.Contracts;
using Web.ViewModels.Company;

[TestFixture]
public class CompanyRetrievalTests
{
    private Mock<IUnitOfWork>? _mockUnitOfWork;

    private Company? _existingCompany;
    private List<Company>? _allCompanies;

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

        this._allCompanies = new List<Company>()
        {
            new Company()
            {
                Name = "Name1",
                Email = "Email1",
                UIC = "UIC1",
            },
            new Company()
            {
                Name = "Name2",
                Email = "Email2",
                UIC = "UIC2",
            },
            new Company()
            {
                Name = "Name3",
                Email = "Email3",
                UIC = "UIC3",
            },
        };

        this._mockUnitOfWork.Setup(uow => uow
            .CompanyRepository
            .GetAsync(It.IsAny<Expression<Func<Company, bool>>>(),
                It.IsAny<Func<IQueryable<Company>,
                    IOrderedQueryable<Company>>>(),
                It.IsAny<string>()))
            .ReturnsAsync(this._allCompanies);

        this._mockUnitOfWork.Setup(uow => uow
                .CompanyRepository
                .GetByIdAsync(this._existingCompany!.Id))
            .ReturnsAsync(this._existingCompany);
    }

    [Test]
    public async Task GetAllAsync_ShouldReturnAllCompanies()
    {
        //Arrange
        ICompanyRetrievalService service
            = new CompanyRetrievalService(this._mockUnitOfWork!.Object);

        int pageIndex = 0;
        int pageSize = 3;
        string searchTerm = string.Empty;

        //Act
        PaginatedList<AllCompaniesViewModel> companies =
            await service.GetAllAsync(pageIndex, pageSize, searchTerm);

        //Assert
        Assert.That(companies.Count, Is.EqualTo(pageSize));
        Assert.That(companies.Count, Is.EqualTo(this._allCompanies!.Count));


        for (int i = 0; i < pageSize; i++)
        {
            AllCompaniesViewModel firstCompany = companies[i];
            Company secondCompany = this._allCompanies![i];

            Assert.That(firstCompany.Id, Is.EqualTo(secondCompany.Id));
            Assert.That(firstCompany.Name, Is.EqualTo(secondCompany.Name));
            Assert.That(firstCompany.Email, Is.EqualTo(secondCompany.Email));
            Assert.That(firstCompany.UIC, Is.EqualTo(secondCompany.UIC));
        }
    }

    [Test]
    public async Task GetAllListAsync_ShouldReturnAllCompanies()
    {
        //Arrange
        ICompanyRetrievalService service
            = new CompanyRetrievalService(this._mockUnitOfWork!.Object);

        //Act
        IEnumerable<AllCompaniesListViewModel>? companies =
            await service.GetAllListAsync();

        //Assert
        foreach (AllCompaniesListViewModel company in companies)
        {
            Assert.IsTrue(this._allCompanies!
                .Any(c => c.Name == company.Name));
        }
    }

    [Test]
    public async Task GetCompanyForEditAsync_ShouldReturnCorrectCompany()
    {
        //Arrange
        ICompanyRetrievalService service
            = new CompanyRetrievalService(this._mockUnitOfWork!.Object);

        //Act
        EditCompanyViewModel companyModel
            = await service.GetCompanyForEditAsync(this._existingCompany!.Id);

        //Assert
        Assert.That(companyModel.Id, Is.EqualTo(this._existingCompany!.Id));
        Assert.That(companyModel.Name, Is.EqualTo(this._existingCompany!.Name));
        Assert.That(companyModel.Email, Is.EqualTo(this._existingCompany!.Email));
        Assert.That(companyModel.UIC, Is.EqualTo(this._existingCompany!.UIC));
    }

    [Test]
    public void GetCompanyForEditAsync_ShouldThrowCompanyNotFoundException()
    {
        //Arrange
        ICompanyRetrievalService service
            = new CompanyRetrievalService(this._mockUnitOfWork!.Object);

        //Act & Assert
        Assert.ThrowsAsync<CompanyNotFoundException>(async () =>
            await service.GetCompanyForEditAsync(Guid.NewGuid()));
    }

    [Test]
    public async Task GetCompanyForDeleteAsync_ShouldReturnCorrectCompany()
    {
        //Arrange
        ICompanyRetrievalService service
            = new CompanyRetrievalService(this._mockUnitOfWork!.Object);

        //Act
        DeleteCompanyViewModel companyModel =
            await service.GetCompanyForDeleteAsync(this._existingCompany!.Id);

        //Assert
        Assert.That(companyModel.Id, Is.EqualTo(this._existingCompany!.Id));
        Assert.That(companyModel.Name, Is.EqualTo(this._existingCompany!.Name));
        Assert.That(companyModel.Email, Is.EqualTo(this._existingCompany!.Email));
        Assert.That(companyModel.UIC, Is.EqualTo(this._existingCompany!.UIC));
    }

    [Test]
    public void GetCompanyForDeleteAsync_ShouldTrowCompanyNotFoundException()
    {
        //Arrange
        ICompanyRetrievalService service
            = new CompanyRetrievalService(this._mockUnitOfWork!.Object);

        //Act & Assert
        Assert.ThrowsAsync<CompanyNotFoundException>(async () =>
            await service.GetCompanyForDeleteAsync(Guid.NewGuid()));
    }

    [Test]
    public void GetCompanyForCreateAsync_ShouldReturnCorrectCompany()
    {
        //Arrange
        ICompanyRetrievalService service
            = new CompanyRetrievalService(this._mockUnitOfWork!.Object);

        //Act
        CreateCompanyViewModel companyModel =
            service.GetCompanyForCreate();

        //Assert
        Assert.That(companyModel.Name, Is.EqualTo(string.Empty));
        Assert.That(companyModel.Email, Is.EqualTo(string.Empty));
        Assert.That(companyModel.UIC, Is.EqualTo(string.Empty));
    }

    [Test]
    public async Task CompanyExistsAsync_ShouldReturnTrue()
    {
        //Arrange
        ICompanyRetrievalService service
            = new CompanyRetrievalService(this._mockUnitOfWork!.Object);

        Guid companyId = this._allCompanies![0].Id;
        this._mockUnitOfWork!.Setup(uow => uow
                .CompanyRepository
                .GetFirstOrDefaultByFilterAsync(It.IsAny<Expression<Func<Company, bool>>>(),
                    It.IsAny<bool>()))
            .ReturnsAsync(this._allCompanies!
                .FirstOrDefault(c => c.Id == companyId));

        //Act & Assert
        Assert.IsTrue(await service.CompanyExistsAsync(companyId));
    }

    [Test]
    public async Task CompanyExistsAsync_ShouldReturnFalse()
    {
        //Arrange
        ICompanyRetrievalService service
            = new CompanyRetrievalService(this._mockUnitOfWork!.Object);

        Guid companyId = Guid.NewGuid();
        this._mockUnitOfWork!.Setup(uow => uow
                .CompanyRepository
                .GetFirstOrDefaultByFilterAsync(It.IsAny<Expression<Func<Company, bool>>>(),
                    It.IsAny<bool>()))
            .ReturnsAsync(this._allCompanies!
                .FirstOrDefault(c => c.Id == companyId));

        //Act & Assert
        Assert.IsFalse(await service.CompanyExistsAsync(companyId));
    }
}