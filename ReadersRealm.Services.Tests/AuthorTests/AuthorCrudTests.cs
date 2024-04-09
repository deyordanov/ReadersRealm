    namespace ReadersRealm.Services.Tests.AuthorTests;

using Common.Exceptions.Author;
using Data.AuthorServices;
using Data.AuthorServices.Contracts;
using Moq;
using ReadersRealm.Data.Models;
using ReadersRealm.Data.Repositories.Contracts;
using Web.ViewModels.Author;

[TestFixture]
public class AuthorCrudTests
{
    private Mock<IUnitOfWork>? _mockUnitOfWork;
    private Author? _existingAuthor;

    [SetUp]
    public void SetUp()
    {
        this._mockUnitOfWork = new Mock<IUnitOfWork>();

        this._existingAuthor = new Author()
        {
            FirstName = "FirstName",
            LastName = "LastName",
            Email = "Email",
            PhoneNumber = "PhoneNumber",
        };

        this._mockUnitOfWork.Setup(uow => uow
                .AuthorRepository
                .GetByIdAsync(this._existingAuthor.Id))
            .ReturnsAsync(this._existingAuthor);

        this._mockUnitOfWork.Setup(uow => uow
                .AuthorRepository
                .AddAsync(It.IsAny<Author>()))
            .Returns(Task.CompletedTask);

        this._mockUnitOfWork.Setup(uow => uow
                .SaveAsync())
            .Returns(Task.CompletedTask);
    }

    [Test]
    public async Task CreateAuthorAsync_ShouldCreateAuthor()
    {
        //Arrange
        IAuthorCrudService service = new AuthorCrudService(this._mockUnitOfWork!.Object);

        CreateAuthorViewModel authorModel = new CreateAuthorViewModel()
        {
            FirstName = this._existingAuthor!.FirstName,
            LastName = this._existingAuthor!.LastName,
            Email = this._existingAuthor!.Email,
            PhoneNumber = this._existingAuthor!.PhoneNumber,
        };

        //Act
        await service.CreateAuthorAsync(authorModel);

        //Assert
        this._mockUnitOfWork.Verify(uow => uow.SaveAsync(), Times.Once());

        this._mockUnitOfWork.Verify(uow => uow
            .AuthorRepository
            .AddAsync(It.Is<Author>(a =>
            a.FirstName == authorModel.FirstName &&
            a.MiddleName == authorModel.MiddleName &&
            a.LastName == authorModel.LastName &&
            a.Email == authorModel.Email &&
            a.PhoneNumber == authorModel.PhoneNumber &&
            a.Age == authorModel.Age &&
            a.Gender == authorModel.Gender
        )), Times.Once);
    }

    [Test]
    public async Task EditAuthorAsync_ShouldUpdateAuthorCorrectly()
    {
        //Arrange
        IAuthorCrudService service = new AuthorCrudService(this._mockUnitOfWork!.Object);

        EditAuthorViewModel authorModel = new EditAuthorViewModel()
        {
            Id = this._existingAuthor!.Id,
            FirstName = "UpdatedFirstName",
            LastName = "UpdatedLastName",
            Email = "UpdatedEmail",
            PhoneNumber = "UpdatedPhoneNumber",
        };

        //Act
        await service.EditAuthorAsync(authorModel);

        //Assert
        this._mockUnitOfWork.Verify(uow => uow.SaveAsync(), Times.Once());

        Assert.That(authorModel.Id, Is.EqualTo(this._existingAuthor.Id));
        Assert.That(authorModel.FirstName, Is.EqualTo(this._existingAuthor.FirstName));
        Assert.That(authorModel.LastName, Is.EqualTo(this._existingAuthor.LastName));
        Assert.That(authorModel.Email, Is.EqualTo(this._existingAuthor.Email));
        Assert.That(authorModel.PhoneNumber, Is.EqualTo(this._existingAuthor.PhoneNumber));
    }

    [Test]
    public void EditAuthorAsync_ShouldThrowAuthorNotFoundException()
    {
        //Arrange
        IAuthorCrudService service = new AuthorCrudService(this._mockUnitOfWork!.Object);

        EditAuthorViewModel authorModel = new EditAuthorViewModel()
        {
            Id = Guid.NewGuid(),
            FirstName = "UpdatedFirstName",
            LastName = "UpdatedLastName",
            Email = "UpdatedEmail",
            PhoneNumber = "UpdatedPhoneNumber",
        };

        //Act & Assert
        Assert.ThrowsAsync<AuthorNotFoundException>(async () =>
            await service.EditAuthorAsync(authorModel));
    }

    [Test]
    public async Task DeleteAuthorAsync_ShouldDeleteUser()
    {
        //Arrange
        IAuthorCrudService service = new AuthorCrudService(this._mockUnitOfWork!.Object);

        DeleteAuthorViewModel authorModel = new DeleteAuthorViewModel()
        {
            Id = this._existingAuthor!.Id,
            FirstName = this._existingAuthor!.FirstName,
            LastName = this._existingAuthor!.LastName,
            Email = this._existingAuthor!.Email,
            PhoneNumber = this._existingAuthor!.PhoneNumber,
        };

        //Act
        await service.DeleteAuthorAsync(authorModel);

        //Assert
        this._mockUnitOfWork.Verify(uow => uow.SaveAsync(), Times.Once);

        this._mockUnitOfWork.Verify(uow => uow
            .AuthorRepository
            .Delete(It.Is<Author>(a =>
                a.Id == authorModel.Id && 
                a.FirstName == authorModel.FirstName &&
                a.LastName == authorModel.LastName &&
                a.Email == authorModel.Email &&
                a.PhoneNumber == authorModel.PhoneNumber)));
    }

    [Test]
    public void DeleteAuthorAsync_ShouldThrowAuthorNotFoundException()
    {
        //Arrange
        IAuthorCrudService service = new AuthorCrudService(this._mockUnitOfWork!.Object);

        DeleteAuthorViewModel authorModel = new DeleteAuthorViewModel()
        {
            Id = Guid.NewGuid(),
            FirstName = this._existingAuthor!.FirstName,
            LastName = this._existingAuthor!.LastName,
            Email = this._existingAuthor!.Email,
            PhoneNumber = this._existingAuthor!.PhoneNumber,
        };

        //Act
        Assert.ThrowsAsync<AuthorNotFoundException>(async () =>
            await service.DeleteAuthorAsync(authorModel));
    }
}