namespace ReadersRealm.Services.Tests.OrderDetailsTests;

using Data.OrderDetailsServices;
using Data.OrderDetailsServices.Contracts;
using Moq;
using ReadersRealm.Data.Models;
using ReadersRealm.Data.Repositories.Contracts;
using Data.OrderDetailsServices.Contracts;
using Data.OrderDetailsServices;
using Web.ViewModels.OrderDetails;
using Web.ViewModels.Order;
using Web.ViewModels.OrderDetails;

public class OrderDetailsCrudTests
{
    private Mock<IUnitOfWork>? _mockUnitOfWork;
    private Mock<IOrderDetailsRetrievalService> _mockOrderDetailsRetrievalService;

    private OrderDetails? _existingOrderDetails;

    [SetUp]
    public void SetUp()
    {
        this._mockUnitOfWork = new Mock<IUnitOfWork>();
        this._mockOrderDetailsRetrievalService = new Mock<IOrderDetailsRetrievalService>();

        this._existingOrderDetails = new OrderDetails()
        {
            Id = Guid.NewGuid(),
            BookId = Guid.NewGuid(),
            OrderHeaderId = Guid.NewGuid(),
            Count = 1,
            Price = 1,
            Order = new Order()
            {
                Id = Guid.NewGuid(),
            },
        };

        this._mockUnitOfWork.Setup(uow => uow
                       .OrderDetailsRepository
                       .AddAsync(It.IsAny<OrderDetails>()))
            .Returns(Task.CompletedTask);

        this._mockUnitOfWork.Setup(uow => uow
                              .OrderDetailsRepository
                              .GetByIdAsync(this._existingOrderDetails!.Id))
            .ReturnsAsync(this._existingOrderDetails);

        this._mockUnitOfWork.Setup(uow => uow
            .SaveAsync())
            .Returns(Task.CompletedTask);
    }

    [Test]
    public async Task CreateOrderDetailsAsync_ShouldCreateOrderDetails()
    {
        //Arrange
        IOrderDetailsCrudService service
            = new OrderDetailsCrudService(
                this._mockUnitOfWork!.Object,
                this._mockOrderDetailsRetrievalService!.Object);

        OrderDetailsViewModel orderDetailsModel = new OrderDetailsViewModel()
        {
            BookId = this._existingOrderDetails!.BookId,
            OrderHeaderId = this._existingOrderDetails!.OrderHeaderId,
            Count = this._existingOrderDetails!.Count,
            Price = this._existingOrderDetails!.Price,
            Order = new OrderViewModel()
            {
                Id = this._existingOrderDetails!.Order!.Id,
            },
        };

        //Act
        await service.CreateOrderDetailsAsync(orderDetailsModel);

        //Assert
        this._mockUnitOfWork.Verify(uow => uow
            .OrderDetailsRepository
            .AddAsync(It.Is<OrderDetails>(od => 
                od.BookId == this._existingOrderDetails!.BookId &&
                od.OrderHeaderId == this._existingOrderDetails!.OrderHeaderId &&
                od.Count == this._existingOrderDetails!.Count &&
                od.Price == this._existingOrderDetails!.Price &&
                od.OrderId == this._existingOrderDetails!.Order!.Id)));

        this._mockUnitOfWork.Verify(uow => uow.SaveAsync(), Times.Once());
    }

    [Test]
    public async Task DeleteOrderDetailsRangeByOrderHeaderIdAsync_ShouldDeleteOrderDetailsRange()
    {
        //Arrange
        IOrderDetailsCrudService service
            = new OrderDetailsCrudService(
                               this._mockUnitOfWork!.Object,
                               this._mockOrderDetailsRetrievalService!.Object);

        IEnumerable<OrderDetailsViewModel> orderDetailsModelList = new List<OrderDetailsViewModel>()
        {
            new OrderDetailsViewModel()
            {
                Id = Guid.NewGuid(),
            },
            new OrderDetailsViewModel()
            {
                Id = Guid.NewGuid(),
            },
        };

        this._mockOrderDetailsRetrievalService.Setup(odrs => odrs
                   .GetAllByOrderHeaderIdAsync(this._existingOrderDetails!.OrderHeaderId))
            .ReturnsAsync(orderDetailsModelList);

        //Act
        await service.DeleteOrderDetailsRangeByOrderHeaderIdAsync(this._existingOrderDetails!.OrderHeaderId);

        //Assert
        this._mockUnitOfWork.Verify(uow => uow
                   .OrderDetailsRepository
                   .DeleteRange(It.Is<IEnumerable<OrderDetails>>(od => 
                                      od.Count() == orderDetailsModelList.Count())));

        this._mockUnitOfWork.Verify(uow => uow.SaveAsync(), Times.Once());
    }
}