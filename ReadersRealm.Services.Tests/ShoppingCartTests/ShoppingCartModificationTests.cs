namespace ReadersRealm.Services.Tests.ShoppingCartTests;

using Common.Exceptions.ShoppingCart;
using Data.ShoppingCartServices;
using Data.ShoppingCartServices.Contracts;
using Moq;
using ReadersRealm.Data.Models;
using ReadersRealm.Data.Repositories.Contracts;

[TestFixture]
public class ShoppingCartModificationTests
{
    private Mock<IUnitOfWork> _mockUnitOfWork;
    private Mock<IShoppingCartCrudService> _mockShoppingCartCrudService;

    private ShoppingCart? _existingShoppingCart;

    [SetUp]
    public void SetUp()
    {
        this._mockUnitOfWork = new Mock<IUnitOfWork>();
        this._mockShoppingCartCrudService = new Mock<IShoppingCartCrudService>();

        this._existingShoppingCart = new ShoppingCart
        {
            Id = Guid.NewGuid(),
            Count = 10
        };

        this._mockUnitOfWork.Setup(uow => uow
                .ShoppingCartRepository
                .GetByIdAsync(this._existingShoppingCart!.Id))
            .ReturnsAsync(this._existingShoppingCart);
    }

    [Test]
    public async Task IncreaseShoppingCartQuantityAsync_ShouldIncreaseShoppingCartCount()
    {
        // Arrange
        ShoppingCartModificationService service 
            = new ShoppingCartModificationService(
                this._mockUnitOfWork.Object,
                this._mockShoppingCartCrudService.Object);

        int increasedShoppingCartQuantity = this._existingShoppingCart!.Count + 1;

        // Act
        await service.IncreaseShoppingCartQuantityAsync(this._existingShoppingCart!.Id);

        // Assert
        Assert.That(increasedShoppingCartQuantity, Is.EqualTo(this._existingShoppingCart!.Count));
    }

    [Test]
    public void IncreaseShoppingCartQuantityAsync_ShouldThrowShoppingCartNotFoundException()
    {
        // Arrange
        ShoppingCartModificationService service
            = new ShoppingCartModificationService(
                this._mockUnitOfWork.Object,
                this._mockShoppingCartCrudService.Object);

        // Act && Assert
        Assert.ThrowsAsync<ShoppingCartNotFoundException>(async () => 
            await service.IncreaseShoppingCartQuantityAsync(Guid.NewGuid()));
    }

    [Test]
    public async Task DecreaseShoppingCartQuantityAsync_ShouldDecreaseShoppingCartCount()
    {
        // Arrange
        ShoppingCartModificationService service
            = new ShoppingCartModificationService(
                this._mockUnitOfWork.Object,
                this._mockShoppingCartCrudService.Object);

        int decreasedShoppingCartQuantity = this._existingShoppingCart!.Count - 1;

        // Act
        await service.DecreaseShoppingCartQuantityAsync(this._existingShoppingCart!.Id);

        // Assert
        Assert.That(decreasedShoppingCartQuantity, Is.EqualTo(this._existingShoppingCart!.Count));
    }

    [Test]
    public async Task DecreaseShoppingCartQuantityAsync_ShouldDeleteShoppingCart_AndReturnTrue()
    {
        // Arrange
        ShoppingCartModificationService service
            = new ShoppingCartModificationService(
                               this._mockUnitOfWork.Object,
                                              this._mockShoppingCartCrudService.Object);

        this._existingShoppingCart!.Count = 1;

        // Act
        bool isDeleted = await service.DecreaseShoppingCartQuantityAsync(this._existingShoppingCart!.Id);

        // Assert
        this._mockShoppingCartCrudService.Verify(sccs => sccs
                   .DeleteShoppingCartAsync(this._existingShoppingCart!.Id), Times.Once);

        Assert.That(isDeleted, Is.True);
    }

    [Test]
    public async Task DecreaseShoppingCartQuantityAsync_ShouldNotDeleteShoppingCart_AndReturnFalse()
    {
        // Arrange
        ShoppingCartModificationService service
            = new ShoppingCartModificationService(
                this._mockUnitOfWork.Object,
                this._mockShoppingCartCrudService.Object);

        // Act
        bool isDeleted = await service.DecreaseShoppingCartQuantityAsync(this._existingShoppingCart!.Id);

        // Assert
        this._mockShoppingCartCrudService.Verify(sccs => sccs
            .DeleteShoppingCartAsync(this._existingShoppingCart!.Id), Times.Never);

        Assert.That(isDeleted, Is.False);
    }

    [Test]
    public void DecreaseShoppingCartQuantityAsync_ShouldThrowShoppingCartNotFoundException()
    {
        // Arrange
        ShoppingCartModificationService service
            = new ShoppingCartModificationService(
                this._mockUnitOfWork.Object,
                this._mockShoppingCartCrudService.Object);

        // Act && Assert
        Assert.ThrowsAsync<ShoppingCartNotFoundException>(async () =>
            await service.DecreaseShoppingCartQuantityAsync(Guid.NewGuid()));
    }
}