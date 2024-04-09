namespace ReadersRealm.Services.Tests.AuthorTests;

using System.Linq.Expressions;
using Common;
using Common.Exceptions.Author;
using Data.AuthorServices;
using Data.AuthorServices.Contracts;
using Moq;
using ReadersRealm.Data.Models;
using ReadersRealm.Data.Repositories.Contracts;
using Web.ViewModels.Author;
using Web.ViewModels.Book;

[TestFixture]
public class AuthorRetrievalTests
{
    private Mock<IUnitOfWork>? _mockUnitOfWork;

    private Author? _existingAuthor;
    private List<Author>? _allAuthors;

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

        this._allAuthors = new List<Author>()
        {
            new Author()
            {
                FirstName = "FirstName1",
                LastName = "LastName1",
                Email = "Email1",
                PhoneNumber = "PhoneNumber1",
                Books = new HashSet<Book>()
                {
                    new Book()
                    {
                        Title = "Book1",
                        ISBN = "ISBN1",
                    },
                },
            },
            new Author()
            {
                FirstName = "FirstName2",
                LastName = "LastName2",
                Email = "Email2",
                PhoneNumber = "PhoneNumber2",
                Books = new HashSet<Book>()
                {
                    new Book()
                    {
                        Title = "Book2",
                        ISBN = "ISBN2",
                    },
                },
            },
            new Author()
            {
                FirstName = "FirstName3",
                LastName = "LastName3",
                Email = "Email3",
                PhoneNumber = "PhoneNumber3",
                Books = new HashSet<Book>()
                {
                    new Book()
                    {
                        Title = "Book3",
                        ISBN = "ISBN3",
                    },
                },
            },
        };

        this._mockUnitOfWork.Setup(uow => uow
            .AuthorRepository
            .GetAsync(It.IsAny<Expression<Func<Author, bool>>>(),
                It.IsAny<Func<IQueryable<Author>,
                    IOrderedQueryable<Author>>>(),
                It.IsAny<string>()))
            .ReturnsAsync(this._allAuthors);

        this._mockUnitOfWork.Setup(uow => uow
                .AuthorRepository
                .GetByIdAsync(this._existingAuthor.Id))
            .ReturnsAsync(this._existingAuthor);

        this._mockUnitOfWork.Setup(uow => uow
                .AuthorRepository
                .GetFirstOrDefaultWithFilterAsync(It.IsAny<Expression<Func<Author, bool>>>()
                    , It.IsAny<bool>()))
            .ReturnsAsync(this._existingAuthor);
    }

    [Test]
    public async Task GetAllAsync_ShouldReturnAllAuthors()
    {
        //Arrange
        IAuthorRetrievalService service 
            = new AuthorRetrievalService(this._mockUnitOfWork!.Object);

        int pageIndex = 0;
        int pageSize = this._allAuthors!.Count;
        string searchTerm = string.Empty;

        //Act
        PaginatedList<AllAuthorsViewModel> authors 
            = await service.GetAllAsync(pageIndex, pageSize, searchTerm);

        //Assert
        Assert.That(authors.Count, Is.EqualTo(pageSize));
        Assert.That(authors.Count, Is.EqualTo(this._allAuthors.Count));

        for (int i = 0; i < pageSize; i++)
        {
            AllAuthorsViewModel firstAuthor = authors[i];
            Author secondAuthor = this._allAuthors[i];

            Assert.That(firstAuthor.FirstName, Is.EqualTo(secondAuthor.FirstName));

            Assert.That(firstAuthor.Books.Count(), Is.EqualTo(secondAuthor.Books.Count));

            int bookCount = firstAuthor.Books.Count();
            foreach (BookViewModel book in firstAuthor.Books)
            {
                Assert.IsTrue(secondAuthor
                    .Books
                    .Any(b => b.Title == book.Title));
            }
        }
    }

    [Test]
    public async Task GetAllListAsync_ShouldReturnAllAuthors()
    {
        //Arrange
        IAuthorRetrievalService service
            = new AuthorRetrievalService(this._mockUnitOfWork!.Object);

        //Act
        List<AllAuthorsListViewModel> authors
            = await service.GetAllListAsync();

        //Assert
        Assert.That(authors.Count, Is.EqualTo(this._allAuthors!.Count));

        for (int i = 0; i < authors.Count; i++)
        {
            Assert.That(authors[i].FirstName, Is.EqualTo(this._allAuthors[i].FirstName));
        }
    }

    [Test]
    public void GetAuthorForCreate_ShouldReturnCorrectAuthor()
    {
        //Arrange
        IAuthorRetrievalService service
            = new AuthorRetrievalService(this._mockUnitOfWork!.Object);

        //Act
        CreateAuthorViewModel authorModel =
            service.GetAuthorForCreate();

        //Assert
        Assert.That(authorModel.FirstName, Is.Empty);
        Assert.That(authorModel.LastName, Is.Empty);
        Assert.That(authorModel.Email, Is.Empty);
        Assert.That(authorModel.PhoneNumber, Is.Empty);
    }

    [Test]
    public async Task GetAuthorForEditAsync_ShouldReturnCorrectAuthor()
    {
        //Arrange
        IAuthorRetrievalService service
            = new AuthorRetrievalService(this._mockUnitOfWork!.Object);

        //Act
        EditAuthorViewModel authorModel =
            await service.GetAuthorForEditAsync(this._existingAuthor!.Id);

        //Assert
        Assert.That(authorModel.FirstName, Is.EqualTo(this._existingAuthor!.FirstName));
        Assert.That(authorModel.LastName, Is.EqualTo(this._existingAuthor!.LastName));
        Assert.That(authorModel.Email, Is.EqualTo(this._existingAuthor!.Email));
        Assert.That(authorModel.PhoneNumber, Is.EqualTo(this._existingAuthor!.PhoneNumber));
    }

    [Test]
    public void GetAuthorForEditAsync_ShouldThrowAuthorNotFoundException()
    {
        //Arrange
        IAuthorRetrievalService service
            = new AuthorRetrievalService(this._mockUnitOfWork!.Object);

        //Act
        Assert.ThrowsAsync<AuthorNotFoundException>(async() =>
            await service.GetAuthorForEditAsync(Guid.NewGuid()));
    }

    [Test]
    public async Task GetAuthorForDeleteAsync_ShouldReturnCorrectAuthor()
    {
        //Arrange
        IAuthorRetrievalService service
            = new AuthorRetrievalService(this._mockUnitOfWork!.Object);

        //Act
        DeleteAuthorViewModel authorModel =
            await service.GetAuthorForDeleteAsync(this._existingAuthor!.Id);

        //Assert
        Assert.That(authorModel.FirstName, Is.EqualTo(this._existingAuthor!.FirstName));
        Assert.That(authorModel.LastName, Is.EqualTo(this._existingAuthor!.LastName));
        Assert.That(authorModel.Email, Is.EqualTo(this._existingAuthor!.Email));
        Assert.That(authorModel.PhoneNumber, Is.EqualTo(this._existingAuthor!.PhoneNumber));
    }

    [Test]
    public void GetAuthorForDeleteAsync_ShouldThrowAuthorNotFoundException()
    {
        //Arrange
        IAuthorRetrievalService service
            = new AuthorRetrievalService(this._mockUnitOfWork!.Object);

        //Act
        Assert.ThrowsAsync<AuthorNotFoundException>(async () =>
            await service.GetAuthorForDeleteAsync(Guid.NewGuid()));
    }

    [Test]
    public async Task AuthorExistsAsync_ShouldReturnTrue()
    {
        //Arrange
        IAuthorRetrievalService service
            = new AuthorRetrievalService(this._mockUnitOfWork!.Object);

        //Act & Assert
        Assert.IsTrue(await service.AuthorExistsAsync(this._existingAuthor!.Id));
    }

    [Test]
    public async Task AuthorExistsAsync_ShouldReturnFalse()
    {
        //Arrange
        IAuthorRetrievalService service
            = new AuthorRetrievalService(this._mockUnitOfWork!.Object);

        //Act & Assert
        Assert.IsTrue(await service.AuthorExistsAsync(Guid.NewGuid()));
    }
}