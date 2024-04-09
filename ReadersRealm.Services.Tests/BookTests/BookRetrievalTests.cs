namespace ReadersRealm.Services.Tests.BookTests;

using System.Linq.Expressions;
using AngleSharp.Css.Dom;
using Common;
using Common.Exceptions.Book;
using Data.AuthorServices.Contracts;
using Data.BookServices;
using Data.BookServices.Contracts;
using Data.CategoryServices.Contracts;
using Moq;
using ReadersRealm.Data.Models;
using ReadersRealm.Data.Repositories.Contracts;
using ReadersRealm.Web.ViewModels.Book;
using Web.ViewModels.Author;
using Web.ViewModels.Category;

[TestFixture]
public class BookRetrievalTests
{
    private Mock<IUnitOfWork>? _mockUnitOfWork;
    private Mock<IAuthorRetrievalService>? _mockAuthorRetrievalService;
    private Mock<ICategoryRetrievalService>? _mockCategoryRetrievalService;

    private Book? _existingBook;
    private List<Book>? _allBooks;
    private List<AllAuthorsListViewModel> _allAuthors;
    private List<AllCategoriesListViewModel> _allCategories;

    [SetUp]
    public void SetUp()
    {
        this._mockUnitOfWork = new Mock<IUnitOfWork>();
        this._mockAuthorRetrievalService = new Mock<IAuthorRetrievalService>();
        this._mockCategoryRetrievalService = new Mock<ICategoryRetrievalService>();

        this._existingBook = new Book()
        {
            Title = "Title",
            ISBN = "ISBN",
            Author = new Author()
            {
                FirstName = "FirstName",
                LastName = "LastName",
                Email = "Email",
                PhoneNumber = "PhoneNumber",
            },
            Category = new Category()
            {
                Name = "Name",
            },
        };

        this._allBooks = new List<Book>()
        {
            new Book()
            {
                Title = "Title1",
                ISBN = "ISBN1",
                Author = new Author()
                {
                    FirstName = "FirstName1",
                    LastName = "LastName1",
                    Email = "Email1",
                    PhoneNumber = "PhoneNumber1",
                },
                Category = new Category()
                {
                    Name = "Name1",
                },
            },
            new Book()
            {
                Title = "Title2",
                ISBN = "ISBN2",
                Author = new Author()
                {
                    FirstName = "FirstName2",
                    LastName = "LastName2",
                    Email = "Email2",
                    PhoneNumber = "PhoneNumber2",
                },
                Category = new Category()
                {
                    Name = "Name2",
                },
            },
            new Book()
            {
                Title = "Title3",
                ISBN = "ISBN3",
                Author = new Author()
                {
                    FirstName = "FirstName3",
                    LastName = "LastName3",
                    Email = "Email3",
                    PhoneNumber = "PhoneNumber3",
                },
                Category = new Category()
                {
                    Name = "Name3",
                },
            },
        };

        this._allAuthors = new List<AllAuthorsListViewModel>()
        {
            new AllAuthorsListViewModel()
            {
                FirstName = "FirstName1",
                LastName = "LastName1",
            },
            new AllAuthorsListViewModel()
            {
                FirstName = "FirstName2",
                LastName = "LastName2",
            },
            new AllAuthorsListViewModel()
            {
                FirstName = "FirstName3",
                LastName = "LastName3",
            },
        };

        this._allCategories = new List<AllCategoriesListViewModel>()
        {
            new AllCategoriesListViewModel()
            {
                Name = "Name1",
            },
            new AllCategoriesListViewModel()
            {
                Name = "Name2",
            },
            new AllCategoriesListViewModel()
            {
                Name = "Name3",
            },
        };

        this._mockUnitOfWork.Setup(uow => uow
            .BookRepository
            .GetAsync(It.IsAny<Expression<Func<Book, bool>>>(),
                It.IsAny<Func<IQueryable<Book>,
                    IOrderedQueryable<Book>>>(),
                It.IsAny<string>()))
            .ReturnsAsync(this._allBooks);

        this._mockUnitOfWork.Setup(uow => uow
                .BookRepository
                .GetByIdAsync(this._existingBook!.Id))
            .ReturnsAsync(this._existingBook);

        this._mockAuthorRetrievalService.Setup(ars => ars
                .GetAllListAsync())
            .ReturnsAsync(this._allAuthors);

        this._mockCategoryRetrievalService.Setup(crs => crs
                .GetAllListAsync())
            .ReturnsAsync(this._allCategories);

        this._mockUnitOfWork.Setup(uow => uow
            .BookRepository
            .GetByIdWithNavPropertiesAsync(
                this._existingBook!.Id, It.IsAny<string>()))
            .ReturnsAsync(this._existingBook);
    }

    [Test]
    public async Task GetAllAsync_ShouldReturnAllBooks()
    {
        //Arrange
        IBookRetrievalService service
            = new BookRetrievalService(
                this._mockUnitOfWork!.Object,
                this._mockAuthorRetrievalService!.Object,
                this._mockCategoryRetrievalService!.Object);

        int pageIndex = 0;
        int pageSize = 3;
        string searchTerm = string.Empty;

        //Act
        PaginatedList<AllBooksViewModel> books =
            await service.GetAllAsync(pageIndex, pageSize, searchTerm);

        //Assert
        Assert.That(books.Count, Is.EqualTo(pageSize));
        Assert.That(books.Count, Is.EqualTo(this._allBooks!.Count));


        for (int i = 0; i < pageSize; i++)
        {
            AllBooksViewModel firstBook = books[i];
            Book secondBook = this._allBooks![i];

            Assert.That(firstBook.Id, Is.EqualTo(secondBook.Id));
            Assert.IsNotNull(firstBook.Author);
            Assert.That(firstBook.Author.Id, Is.EqualTo(secondBook.Author.Id));
            Assert.IsNotNull(firstBook.Category);
            Assert.That(firstBook.Category.Id, Is.EqualTo(secondBook.Category.Id));
        }
    }

    [Test]
    public async Task GetBookForEditAsync_ShouldReturnCorrectBook()
    {
        //Arrange
        IBookRetrievalService service
            = new BookRetrievalService(
                this._mockUnitOfWork!.Object,
                this._mockAuthorRetrievalService!.Object,
                this._mockCategoryRetrievalService!.Object);

        int authorsCount = this._allAuthors!.Count;
        int categoriesCount = this._allCategories!.Count;

        //Act
        EditBookViewModel bookModel 
            = await service.GetBookForEditAsync(this._existingBook!.Id);

        //Assert
        Assert.That(bookModel.Id, Is.EqualTo(this._existingBook!.Id));
        Assert.That(bookModel.Title, Is.EqualTo(this._existingBook!.Title));
        Assert.That(bookModel.ISBN, Is.EqualTo(this._existingBook!.ISBN));
        
        CollectionAssert.AreEqual(bookModel.AuthorsList, this._allAuthors);
        CollectionAssert.AreEqual(bookModel.CategoriesList, this._allCategories);
    }

    [Test]
    public void GetBookForEditAsync_ShouldThrowBookNotFoundException()
    {
        //Arrange
        IBookRetrievalService service
            = new BookRetrievalService(
                this._mockUnitOfWork!.Object,
                this._mockAuthorRetrievalService!.Object,
                this._mockCategoryRetrievalService!.Object);

        //Act & Assert
        Assert.ThrowsAsync<BookNotFoundException>(async () =>
            await service.GetBookForEditAsync(Guid.NewGuid()));
    }

    [Test]
    public async Task GetBookForDeleteAsync_ShouldReturnCorrectBook()
    {
        //Arrange
        IBookRetrievalService service
            = new BookRetrievalService(
                this._mockUnitOfWork!.Object,
                this._mockAuthorRetrievalService!.Object,
                this._mockCategoryRetrievalService!.Object);

        //Act
        DeleteBookViewModel bookModel =
            await service.GetBookForDeleteAsync(this._existingBook!.Id);

        //Assert
        Assert.That(bookModel.Id, Is.EqualTo(this._existingBook!.Id));
        Assert.That(bookModel.Title, Is.EqualTo(this._existingBook!.Title));
        Assert.That(bookModel.ISBN, Is.EqualTo(this._existingBook!.ISBN));
        Assert.That(bookModel.Author.Id, Is.EqualTo(this._existingBook!.Author.Id));
        Assert.That(bookModel.Category.Id, Is.EqualTo(this._existingBook!.Category.Id));
    }

    [Test]
    public void GetBookForDeleteAsync_ShouldTrowBookNotFoundException()
    {
        //Arrange
        IBookRetrievalService service
            = new BookRetrievalService(
                this._mockUnitOfWork!.Object,
                this._mockAuthorRetrievalService!.Object,
                this._mockCategoryRetrievalService!.Object);

        //Act & Assert
        Assert.ThrowsAsync<BookNotFoundException>(async () =>
            await service.GetBookForDeleteAsync(Guid.NewGuid()));
    }

    [Test]
    public async Task GetBookForDetailsAsync_ShouldReturnCorrectBook()
    {
        //Arrange
        IBookRetrievalService service
            = new BookRetrievalService(
                this._mockUnitOfWork!.Object,
                this._mockAuthorRetrievalService!.Object,
                this._mockCategoryRetrievalService!.Object);

        //Act
        DetailsBookViewModel bookModel =
            await service.GetBookForDetailsAsync(this._existingBook!.Id);

        //Assert
        Assert.That(bookModel.Id, Is.EqualTo(this._existingBook!.Id));
        Assert.That(bookModel.Title, Is.EqualTo(this._existingBook!.Title));
        Assert.That(bookModel.ISBN, Is.EqualTo(this._existingBook!.ISBN));
        Assert.That(bookModel.Author.Id, Is.EqualTo(this._existingBook!.Author.Id));
        Assert.That(bookModel.Category.Id, Is.EqualTo(this._existingBook!.Category.Id));
    }

    [Test]
    public void GetBookForDetailsAsync_ShouldTrowBookNotFoundException()
    {
        //Arrange
        IBookRetrievalService service
            = new BookRetrievalService(
                this._mockUnitOfWork!.Object,
                this._mockAuthorRetrievalService!.Object,
                this._mockCategoryRetrievalService!.Object);

        //Act & Assert
        Assert.ThrowsAsync<BookNotFoundException>(async () =>
            await service.GetBookForDetailsAsync(Guid.NewGuid()));
    }

    [Test]
    public async Task GetBookForCreateAsync_ShouldReturnCorrectBook()
    {
        //Arrange
        IBookRetrievalService service
            = new BookRetrievalService(
                this._mockUnitOfWork!.Object,
                this._mockAuthorRetrievalService!.Object,
                this._mockCategoryRetrievalService!.Object);

        //Act
        CreateBookViewModel bookModel =
            await service.GetBookForCreateAsync();

        //Assert
        Assert.That(bookModel.Title, Is.EqualTo(string.Empty));
        Assert.That(bookModel.ISBN, Is.EqualTo(string.Empty));

        CollectionAssert.AreEqual(bookModel.AuthorsList, this._allAuthors);
        CollectionAssert.AreEqual(bookModel.CategoriesList, this._allCategories);
    }
}