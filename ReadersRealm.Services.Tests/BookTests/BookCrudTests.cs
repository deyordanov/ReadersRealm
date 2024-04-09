namespace ReadersRealm.Services.Tests.BookTests;

using Common.Exceptions.Book;
using Data.BookServices;
using Data.BookServices.Contracts;
using Moq;
using ReadersRealm.Data.Models;
using ReadersRealm.Data.Repositories.Contracts;
using Web.ViewModels.Author;
using Web.ViewModels.Book;
using Web.ViewModels.Category;

[TestFixture]
public class BookCrudTests
{
    private Mock<IUnitOfWork>? _mockUnitOfWork;

    private Book? _existingBook;

    [SetUp]
    public void SetUp()
    {
        this._mockUnitOfWork = new Mock<IUnitOfWork>();

        this._existingBook = new Book()
        {
            Title = "Title",
            ISBN = "ISBN",
        };

        this._mockUnitOfWork.Setup(uow => uow
                .BookRepository
                .AddAsync(It.IsAny<Book>()))
            .Returns(Task.CompletedTask);

        this._mockUnitOfWork.Setup(uow => uow
                .BookRepository
                .GetByIdAsync(this._existingBook!.Id))
            .ReturnsAsync(this._existingBook);
    }

    [Test]
    public async Task CreateBookAsync_ShouldCreateBook()
    {
        //Arrange
        IBookCrudService service
            = new BookCrudService(this._mockUnitOfWork!.Object);

        CreateBookViewModel bookModel = new CreateBookViewModel()
        {
            Title = this._existingBook!.Title,
            ISBN = this._existingBook!.ISBN,
            AuthorsList = new List<AllAuthorsListViewModel>(),
            CategoriesList = new List<AllCategoriesListViewModel>(),
        };

        //Act
        await service.CreateBookAsync(bookModel);

        //Assert
        this._mockUnitOfWork.Verify(uow => uow
            .BookRepository
            .AddAsync(It.Is<Book>(b => 
                b.Title == this._existingBook!.Title &&
                b.ISBN == this._existingBook!.ISBN)));

        this._mockUnitOfWork.Verify(uow => uow.SaveAsync(), Times.Once());
    }

    [Test]
    public async Task EditBookAsync_ShouldUpdateBookCorrectly()
    {
        //Arrange
        IBookCrudService service
            = new BookCrudService(this._mockUnitOfWork!.Object);

        EditBookViewModel bookModel = new EditBookViewModel()
        {
            Id = this._existingBook!.Id,
            Title = "UpdatedTitle",
            ISBN = "UpdatedISBN",
            AuthorsList = new List<AllAuthorsListViewModel>(),
            CategoriesList = new List<AllCategoriesListViewModel>(), 
        };

        //Act
        await service.EditBookAsync(bookModel);

        //Assert
        this._mockUnitOfWork.Verify(uow => uow.SaveAsync(), Times.Once());

        Assert.That(bookModel.Id, Is.EqualTo(this._existingBook!.Id));
        Assert.That(bookModel.Title, Is.EqualTo(this._existingBook!.Title));
        Assert.That(bookModel.ISBN, Is.EqualTo(this._existingBook!.ISBN));
    }

    [Test]
    public void EditBookAsync_ShouldThrowBookNotFoundException()
    {
        //Arrange
        IBookCrudService service
            = new BookCrudService(this._mockUnitOfWork!.Object);

        EditBookViewModel bookModel = new EditBookViewModel()
        {
            Id = Guid.NewGuid(),
            Title = "UpdatedTitle",
            ISBN = "UpdatedISBN",
            AuthorsList = new List<AllAuthorsListViewModel>(),
            CategoriesList = new List<AllCategoriesListViewModel>(),
        };

        //Act & Assert
        Assert.ThrowsAsync<BookNotFoundException>(async () =>
        await service.EditBookAsync(bookModel));
    }

    [Test]
    public async Task DeleteBookAsync_ShouldDeleteBook()
    {
        //Arrange
        IBookCrudService service
            = new BookCrudService(this._mockUnitOfWork!.Object);

        DeleteBookViewModel bookModel = new DeleteBookViewModel()
        {
            Id = this._existingBook!.Id,
            Title = this._existingBook!.Title,
            ISBN = this._existingBook!.ISBN,
        };

        //Act
        await service.DeleteBookAsync(bookModel);

        //Assert
        this._mockUnitOfWork.Verify(uow => uow
            .BookRepository
            .Delete(It.Is<Book>(b => 
                b.Id == this._existingBook!.Id && 
                b.Title == this._existingBook!.Title && 
                b.ISBN == this._existingBook!.ISBN)));

        this._mockUnitOfWork.Verify(uow => uow.SaveAsync(), Times.Once());
    }

    [Test]
    public void DeleteBookAsync_ShouldThrowBookNotFoundException()
    {
        //Arrange
        IBookCrudService service
            = new BookCrudService(this._mockUnitOfWork!.Object);

        DeleteBookViewModel bookModel = new DeleteBookViewModel()
        {
            Id = Guid.NewGuid(),
            Title = this._existingBook!.Title,
            ISBN = this._existingBook!.ISBN,
        };

        //Act & Assert
        Assert.ThrowsAsync<BookNotFoundException>(async () =>
            await service.DeleteBookAsync(bookModel));
    }
}