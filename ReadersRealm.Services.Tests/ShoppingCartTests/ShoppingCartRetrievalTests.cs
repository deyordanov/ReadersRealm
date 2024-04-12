namespace ReadersRealm.Services.Tests.ShoppingCartTests;

using System.Linq.Expressions;
using Data.ApplicationUserServices.Contracts;
using Data.ShoppingCartServices;
using Moq;
using ReadersRealm.Data.Models;
using ReadersRealm.Data.Models.Enums;
using ReadersRealm.Data.Repositories.Contracts;
using Web.ViewModels.Book;
using Web.ViewModels.ShoppingCart;

public class ShoppingCartRetrievalTests
{
    private Mock<IUnitOfWork>? _mockUnitOfWork;
    private Mock<IApplicationUserRetrievalService>? _mockApplicationUserRetrievalService;

    private ShoppingCart? _existingShoppingCart;
    private List<ShoppingCart>? _allShoppingCarts;

    [SetUp]
    public void Setup()
    {
        this._mockUnitOfWork = new Mock<IUnitOfWork>();
        this._mockApplicationUserRetrievalService = new Mock<IApplicationUserRetrievalService>();

        this._existingShoppingCart = new ShoppingCart()
        {
            Id = Guid.NewGuid(),
            ApplicationUserId = Guid.NewGuid(),
            BookId = Guid.NewGuid(),
            Count = 1,
        };

        this._allShoppingCarts = new List<ShoppingCart>()
        {
            new ShoppingCart()
            {
                Id = Guid.NewGuid(),
                ApplicationUserId = this._existingShoppingCart!.ApplicationUserId,
                BookId = this._existingShoppingCart!.BookId,
                Book = new Book()
                {
                    Title = "Title1",
                    ISBN = "ISBN1",
                },
                Count = this._existingShoppingCart!.Count,
            },
            new ShoppingCart()
            {
                Id = Guid.NewGuid(),
                ApplicationUserId = this._existingShoppingCart!.ApplicationUserId,
                BookId = this._existingShoppingCart!.BookId,
                Book = new Book()
                {
                    Title = "Title2",
                    ISBN = "ISBN2",
                },
                Count = this._existingShoppingCart!.Count,
            },
            new ShoppingCart()
            {
                Id = Guid.NewGuid(),
                ApplicationUserId = this._existingShoppingCart!.ApplicationUserId,
                BookId = this._existingShoppingCart!.BookId,
                Book = new Book()
                {
                    Title = "Title3",
                    ISBN = "ISBN3",
                },
                Count = this._existingShoppingCart!.Count,
            },
        };

        this._mockUnitOfWork.Setup(uow => uow
                .ShoppingCartRepository
                .GetAsync(It.IsAny<Expression<Func<ShoppingCart, bool>>>(),
                    It.IsAny<Func<IQueryable<ShoppingCart>,
                        IOrderedQueryable<ShoppingCart>>>(),
                    It.IsAny<string>()))
            .ReturnsAsync(this._allShoppingCarts);

        this._mockUnitOfWork.Setup(uow => uow
                       .ShoppingCartRepository
                       .GetFirstOrDefaultWithFilterAsync(
                           It.IsAny<Expression<Func<ShoppingCart, bool>>>(), 
                           It.IsAny<bool>()))
            .ReturnsAsync(this._existingShoppingCart);
    }

    [Test]
    public void GetShoppingCart_WithValidBookModel_ReturnsShoppingCartViewModel()
    {
        // Arrange
        ShoppingCartRetrievalService shoppingCartRetrievalService 
            = new ShoppingCartRetrievalService(
                this._mockUnitOfWork!.Object, 
                this._mockApplicationUserRetrievalService!.Object);
        
        DetailsBookViewModel bookModel = new DetailsBookViewModel()
        {
            Id = Guid.NewGuid(),
            Title = "Test Book",
            Description = "Test Description",
            Price = 10.00m,
            Pages = 100,
            ISBN = "ISBN",
            AuthorId = Guid.NewGuid(),
            CategoryId = 1,
            ImageId = "ImageId",
            BookCover = BookCover.Hardcover,
            Used = false,
        };

        // Act
        ShoppingCartViewModel shoppingCartModel 
            = shoppingCartRetrievalService.GetShoppingCart(bookModel);

        // Assert
        Assert.That(shoppingCartModel.Book.Id, Is.EqualTo(bookModel.Id));
        Assert.That(shoppingCartModel.Book.Title, Is.EqualTo(bookModel.Title));
        Assert.That(shoppingCartModel.Book.Description, Is.EqualTo(bookModel.Description));
        Assert.That(shoppingCartModel.Book.Price, Is.EqualTo(bookModel.Price));
        Assert.That(shoppingCartModel.Book.Pages, Is.EqualTo(bookModel.Pages));
        Assert.That(shoppingCartModel.Book.ISBN, Is.EqualTo(bookModel.ISBN));
        Assert.That(shoppingCartModel.Book.AuthorId, Is.EqualTo(bookModel.AuthorId));
        Assert.That(shoppingCartModel.Book.CategoryId, Is.EqualTo(bookModel.CategoryId));
        Assert.That(shoppingCartModel.Book.ImageId, Is.EqualTo(bookModel.ImageId));
        Assert.That(shoppingCartModel.Book.BookCover, Is.EqualTo(bookModel.BookCover));
        Assert.That(shoppingCartModel.Book.Used, Is.EqualTo(bookModel.Used));
    }

    [Test]
    public async Task GetShoppingCartCountByApplicationUserIdAsync_ShouldReturnCorrectShoppingCartCount()
    {
        // Arrange
        ShoppingCartRetrievalService service
            = new ShoppingCartRetrievalService(
                this._mockUnitOfWork!.Object,
                this._mockApplicationUserRetrievalService!.Object);

        int allShoppingCartsCount = this._allShoppingCarts!.Count;

        //Act
        int count = await service.GetShoppingCartCountByApplicationUserIdAsync(this._existingShoppingCart!.ApplicationUserId);

        //Assert
        Assert.That(count, Is.EqualTo(allShoppingCartsCount));
    }
}