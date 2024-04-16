namespace ReadersRealm.Services.Tests.ShoppingCartTests;

using Common.Exceptions.ShoppingCart;
using Data.ShoppingCartServices;
using Data.ShoppingCartServices.Contracts;
using Moq;
using ReadersRealm.Data.Models;
using ReadersRealm.Data.Repositories.Contracts;
using Web.ViewModels.ShoppingCart;

[TestFixture]
public class ShoppingCartCrudTests
{
    private Mock<IUnitOfWork>? _mockUnitOfWork;

    private ShoppingCartViewModel? _existingShoppingCartModel;
    private ShoppingCart? _existingShoppingCart;

    [SetUp]
    public void SetUp()
    {
        this._mockUnitOfWork = new Mock<IUnitOfWork>();

        this._existingShoppingCartModel = new ShoppingCartViewModel()
        {
            Id = Guid.NewGuid(),
            ApplicationUserId = Guid.NewGuid(),
            BookId = Guid.NewGuid(),
            Count = 1,
            TotalPrice = 100,
        };

        this._existingShoppingCart = new ShoppingCart()
        {
            Id = this._existingShoppingCartModel.Id,
            ApplicationUserId = this._existingShoppingCartModel.ApplicationUserId,
            BookId = this._existingShoppingCartModel.BookId,
            Count = this._existingShoppingCartModel.Count,
        };

        this._mockUnitOfWork.Setup(uow => uow
                .ShoppingCartRepository
                .AddAsync(It.IsAny<ShoppingCart>()))
            .Returns(Task.CompletedTask);

        this._mockUnitOfWork.Setup(uow => uow
            .SaveAsync())
            .Returns(Task.CompletedTask);

        this._mockUnitOfWork.Setup(uow => uow
                .ShoppingCartRepository
                .GetByApplicationUserIdAndBookIdAsync(this._existingShoppingCart.ApplicationUserId!,
                    this._existingShoppingCart!.BookId))
            .ReturnsAsync(this._existingShoppingCart);

        this._mockUnitOfWork.Setup(uow => uow
                .ShoppingCartRepository
                .GetByIdAsync(this._existingShoppingCart!.Id))
            .ReturnsAsync(this._existingShoppingCart);
    }

    [Test]
    public async Task CreateShoppingCartAsync_ShouldCreateShoppingCart()
    {
        //Arrange
        IShoppingCartCrudService service
            = new ShoppingCartCrudService(this._mockUnitOfWork!.Object);

        //Act
        await service.CreateShoppingCartAsync(this._existingShoppingCartModel!);

        //Assert
        this._mockUnitOfWork.Verify(uow => uow
                   .ShoppingCartRepository
                   .AddAsync(It.Is<ShoppingCart>(sc => 
                       sc.Id == this._existingShoppingCartModel!.Id &&
                       sc.ApplicationUserId == this._existingShoppingCartModel!.ApplicationUserId &&
                       sc.BookId == this._existingShoppingCartModel!.BookId &&
                       sc.Count == this._existingShoppingCartModel!.Count)), Times.Once());

        this._mockUnitOfWork.Verify(uow => uow
            .SaveAsync(), Times.Once());
    }

    [Test]
    public async Task UpdateShoppingCartCountAsync_ShouldUpdateShoppingCartCountCorrectly()
    {
        //Arrange
        IShoppingCartCrudService service
            = new ShoppingCartCrudService(this._mockUnitOfWork!.Object);

        int currentShoppingCartItemCount = this._existingShoppingCartModel!.Count;
        
        //Act
        await service.UpdateShoppingCartCountAsync(this._existingShoppingCartModel);

        //Assert
        Assert.That(this._existingShoppingCartModel.Count, Is.EqualTo(currentShoppingCartItemCount));

        this._mockUnitOfWork.Verify(uow => uow
                   .SaveAsync(), Times.Once());
    }

    [Test]
    public void UpdateShoppingCartCountAsync_ShouldThrowShoppingCartNotFoundException()
    {
        //Arrange
        IShoppingCartCrudService service
            = new ShoppingCartCrudService(this._mockUnitOfWork!.Object);

        int currentShoppingCartItemCount = this._existingShoppingCartModel!.Count;

        ShoppingCartViewModel shoppingCartModel = new ShoppingCartViewModel()
        {
            ApplicationUserId = Guid.NewGuid(),
            BookId = Guid.NewGuid(),
            Count = 1,
        };

        //Act
        Assert.ThrowsAsync<ShoppingCartNotFoundException>(async () =>
            await service.UpdateShoppingCartCountAsync(shoppingCartModel));

    }

    [Test]
    public async Task DeleteShoppingCartAsync_ShouldDeleteShoppingCart()
    {
        //Arrange
        IShoppingCartCrudService service
            = new ShoppingCartCrudService(this._mockUnitOfWork!.Object);

        //Act
        await service.DeleteShoppingCartAsync(this._existingShoppingCartModel!.Id);

        //Assert
        this._mockUnitOfWork.Verify(uow => uow
                          .ShoppingCartRepository
                          .Delete(It.Is<ShoppingCart>(sc =>
                                                    sc.Id == this._existingShoppingCartModel!.Id)), Times.Once());

        this._mockUnitOfWork.Verify(uow => uow
                   .SaveAsync(), Times.Once());
    }

    [Test]
    public void DeleteShoppingCartAsync_ShouldThrowShoppingCartNotFoundException()
    {
        //Arrange
        IShoppingCartCrudService service
            = new ShoppingCartCrudService(this._mockUnitOfWork!.Object);

        //Act
        Assert.ThrowsAsync<ShoppingCartNotFoundException>(async () =>
                       await service.DeleteShoppingCartAsync(Guid.NewGuid()));
    }

    [Test]
    public async Task DeleteAllShoppingCartsApplicationUserIdAsync_ShouldDeleteAllShoppingCarts()
    {
        //Arrange
        IShoppingCartCrudService service
            = new ShoppingCartCrudService(this._mockUnitOfWork!.Object);

        List<ShoppingCart> allShoppingCarts = new List<ShoppingCart>()
        {
            this._existingShoppingCart!,
            new ShoppingCart()
            {
                Id = Guid.NewGuid(),
                ApplicationUserId = this._existingShoppingCartModel!.ApplicationUserId,
                BookId = Guid.NewGuid(),
                Count = 1,
            },
            new ShoppingCart()
            {
                Id = Guid.NewGuid(),
                ApplicationUserId = this._existingShoppingCartModel.ApplicationUserId,
                BookId = Guid.NewGuid(),
                Count = 1,
            },
            new ShoppingCart()
            {
                Id = Guid.NewGuid(),
                ApplicationUserId = this._existingShoppingCartModel.ApplicationUserId,
                BookId = Guid.NewGuid(),
                Count = 1,
            }
        };

        this._mockUnitOfWork.Setup(uow => uow
                   .ShoppingCartRepository
                   .GetAllByApplicationUserIdAsync(this._existingShoppingCart!.ApplicationUserId))
            .ReturnsAsync(allShoppingCarts);

        //Act
        await service.DeleteAllShoppingCartsApplicationUserIdAsync(this._existingShoppingCartModel.ApplicationUserId);

        //Assert
        this._mockUnitOfWork.Verify(uow => uow
                .ShoppingCartRepository
                .DeleteRange(It.Is<List<ShoppingCart>>(carts => carts.SequenceEqual(allShoppingCarts))), Times.Once());

        this._mockUnitOfWork.Verify(uow => uow
                          .SaveAsync(), Times.Once());
    }
}