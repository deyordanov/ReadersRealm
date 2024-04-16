namespace ReadersRealm.Services.Tests.OrderTests;

using System.Linq.Expressions;
using Common.Exceptions.Order;
using Data.OrderDetailsServices.Contracts;
using Data.OrderHeaderServices.Contracts;
using Data.OrderServices;
using Data.OrderServices.Contracts;
using Moq;
using Common;
using ReadersRealm.Data.Models;
using ReadersRealm.Data.Repositories.Contracts;
using Web.ViewModels.Book;
using Web.ViewModels.Author;
using Web.ViewModels.Order;
using Web.ViewModels.OrderDetails;
using Web.ViewModels.OrderHeader;

public class OrderRetrievalTests
{
    private Mock<IUnitOfWork>? _mockUnitOfWork;
    private Mock<IOrderDetailsRetrievalService>? _mockOrderDetailsRetrievalService;
    private Mock<IOrderHeaderRetrievalService>? _mockOrderHeaderRetrievalService;

    private Order? _existingOrder;
    private OrderHeaderViewModel _existingOrderHeader;
    private List<Order>? _allOrders;
    private List<OrderDetailsViewModel> _allOrdersDetails;

    [SetUp]
    public void Setup()
    {
        this._mockUnitOfWork = new Mock<IUnitOfWork>();
        this._mockOrderDetailsRetrievalService = new Mock<IOrderDetailsRetrievalService>();
        this._mockOrderHeaderRetrievalService = new Mock<IOrderHeaderRetrievalService>();

        this._existingOrder = new Order
        {
            Id = Guid.NewGuid(),
            OrderHeaderId = Guid.NewGuid(),
        };

        this._existingOrderHeader = new OrderHeaderViewModel
        {
            Id = Guid.NewGuid(),
            ApplicationUserId = Guid.NewGuid(),
            FirstName = "FirstName",
            LastName = "LastName",
            PhoneNumber = "PhoneNumber",
            City = "City",
            PostalCode = "PostalCode",
            State = "State",
            StreetAddress = "StreetAddress",
        };

        this._allOrders = new List<Order>
        {
            new Order
            {
                Id = Guid.NewGuid(),
                OrderHeaderId = this._existingOrderHeader!.Id,
            },
            new Order
            {
                Id = Guid.NewGuid(),
                OrderHeaderId = this._existingOrderHeader!.Id,
            },
            new Order
            {
                Id = Guid.NewGuid(),
                OrderHeaderId = this._existingOrderHeader!.Id,
            },
        };

        this._allOrdersDetails = new List<OrderDetailsViewModel>
        {
            new OrderDetailsViewModel
            {
                Id = Guid.NewGuid(),
                OrderHeaderId = this._existingOrderHeader!.Id,
                Book = new BookViewModel
                {
                    Id = Guid.NewGuid(),
                    Title = "Title1",
                    Author = new AuthorViewModel()
                    {
                        FirstName = "FirstName1",
                        LastName = "LastName1",
                        Email = "Email1",
                        PhoneNumber = "PhoneNumber1",
                    },
                    Price = 10.0m,
                    ISBN = "ISBN1"
                },
            },
            new OrderDetailsViewModel
            {
                Id = Guid.NewGuid(),
                OrderHeaderId = this._existingOrderHeader!.Id,
                Book = new BookViewModel
                {
                    Id = Guid.NewGuid(),
                    Title = "Title2",
                    Author = new AuthorViewModel()
                    {
                        FirstName = "FirstName2",
                        LastName = "LastName2",
                        Email = "Email2",
                        PhoneNumber = "PhoneNumber2",
                    },
                    Price = 10.0m,
                    ISBN = "ISBN2"
                },
            },
            new OrderDetailsViewModel
            {
                Id = Guid.NewGuid(),
                OrderHeaderId = this._existingOrderHeader!.Id,
                Book = new BookViewModel
                {
                    Id = Guid.NewGuid(),
                    Title = "Title3",
                    Author = new AuthorViewModel()
                    {
                        FirstName = "FirstName3",
                        LastName = "LastName3",
                        Email = "Email3",
                        PhoneNumber = "PhoneNumber3",
                    },
                    Price = 10.0m,
                    ISBN = "ISBN3"
                },
            },
        };

        this._mockUnitOfWork.Setup(uow => uow
                .OrderRepository
                .GetAsync(It.IsAny<Expression<Func<Order, bool>>>(),
                    It.IsAny<Func<IQueryable<Order>,
                        IOrderedQueryable<Order>>>(),
                    It.IsAny<string>()))
            .ReturnsAsync(this._allOrders);

        this._mockOrderHeaderRetrievalService.Setup(ohrs => ohrs
            .GetByIdAsyncWithNavPropertiesAsync(this._existingOrderHeader!.Id))
            .ReturnsAsync(this._existingOrderHeader);

        this._mockOrderDetailsRetrievalService.Setup(odrs => odrs
            .GetAllByOrderHeaderIdAsync(this._existingOrderHeader!.Id))
            .ReturnsAsync(this._allOrdersDetails);

        this._mockUnitOfWork.Setup(uow => uow
            .OrderRepository
            .GetByIdAsync(this._existingOrder!.Id))
            .ReturnsAsync(this._existingOrder);

        this._mockUnitOfWork.Setup(uow => uow
                .OrderRepository
                .GetByOrderHeaderIdAsync(this._existingOrderHeader!.Id))
            .ReturnsAsync(this._existingOrder);
    }

    [Test]
    public async Task GetAllAsync_ShouldReturnAllOrders()
    {
        //Arrange
        IOrderRetrievalService service
            = new OrderRetrievalService(
                this._mockOrderDetailsRetrievalService!.Object,
                this._mockOrderHeaderRetrievalService!.Object,
                this._mockUnitOfWork!.Object);

        int pageIndex = 0;
        int pageSize = 3;
        string searchTerm = string.Empty;

        //Act
        PaginatedList<AllOrdersViewModel> orders 
            = await service.GetAllAsync(pageIndex, pageSize, searchTerm);

        //Assert
        Assert.That(orders.Count, Is.EqualTo(pageSize));
        Assert.That(orders.Count, Is.EqualTo(this._allOrders!.Count));

        for (int i = 0; i < pageSize; i++)
        {
            AllOrdersViewModel firstOrder = orders[i];
            Order secondOrder = this._allOrders![i];

            Assert.IsNotNull(firstOrder.OrderHeaderId);
            Assert.That(firstOrder.OrderHeader.Id, Is.EqualTo(secondOrder.OrderHeaderId));

            foreach (OrderDetailsViewModel firstOrderDetails in firstOrder.OrderDetailsList)
            {
                OrderDetailsViewModel secondOrderDetails = this._allOrdersDetails
                    .First(od => od.Id == firstOrderDetails.Id);

                Assert.IsNotNull(firstOrderDetails.OrderHeaderId);
                Assert.That(firstOrderDetails.OrderHeaderId, Is.EqualTo(secondOrderDetails.OrderHeaderId));
                Assert.IsNotNull(firstOrderDetails.Book);
                Assert.That(firstOrderDetails.Book.Id, Is.EqualTo(secondOrderDetails.Book.Id));
                Assert.That(firstOrderDetails.Book.Title, Is.EqualTo(secondOrderDetails.Book.Title));
                Assert.IsNotNull(firstOrderDetails.Book.Author);
                Assert.That(firstOrderDetails.Book.Author.FirstName, Is.EqualTo(secondOrderDetails.Book.Author.FirstName));
                Assert.That(firstOrderDetails.Book.Author.LastName, Is.EqualTo(secondOrderDetails.Book.Author.LastName));
                Assert.That(firstOrderDetails.Book.Author.Email, Is.EqualTo(secondOrderDetails.Book.Author.Email));
                Assert.That(firstOrderDetails.Book.Author.PhoneNumber, Is.EqualTo(secondOrderDetails.Book.Author.PhoneNumber));
                Assert.That(firstOrderDetails.Book.Price, Is.EqualTo(secondOrderDetails.Book.Price));
                Assert.That(firstOrderDetails.Book.ISBN, Is.EqualTo(secondOrderDetails.Book.ISBN));
            }
        }
    }

    [Test]
    public async Task GetAllByUserIdAsync_ShouldReturnAllOrdersByUserId()
    {
        //Arrange
        IOrderRetrievalService service
            = new OrderRetrievalService(
                               this._mockOrderDetailsRetrievalService!.Object,
                               this._mockOrderHeaderRetrievalService!.Object,
                               this._mockUnitOfWork!.Object);

        int pageIndex = 0;
        int pageSize = 3;
        string searchTerm = string.Empty;
        Guid userId = this._existingOrderHeader!.ApplicationUserId;

        //Act
        PaginatedList<AllOrdersViewModel> orders =
            await service.GetAllByUserIdAsync(pageIndex, pageSize, searchTerm, userId);

        //Assert
        Assert.That(orders.Count, Is.EqualTo(pageSize));
        Assert.That(orders.Count, Is.EqualTo(this._allOrders!.Count));

        for (int i = 0; i < pageSize; i++)
        {
            AllOrdersViewModel firstOrder = orders[i];
            Order secondOrder = this._allOrders![i];

            Assert.IsNotNull(firstOrder.OrderHeaderId);
            Assert.That(firstOrder.OrderHeader.Id, Is.EqualTo(secondOrder.OrderHeaderId));

            foreach (OrderDetailsViewModel firstOrderDetails in firstOrder.OrderDetailsList)
            {
                OrderDetailsViewModel secondOrderDetails = this._allOrdersDetails
                    .First(od => od.Id == firstOrderDetails.Id);

                Assert.IsNotNull(firstOrderDetails.OrderHeaderId);
                Assert.That(firstOrderDetails.OrderHeaderId, Is.EqualTo(secondOrderDetails.OrderHeaderId));
                Assert.IsNotNull(firstOrderDetails.Book);
                Assert.That(firstOrderDetails.Book.Id, Is.EqualTo(secondOrderDetails.Book.Id));
                Assert.That(firstOrderDetails.Book.Title, Is.EqualTo(secondOrderDetails.Book.Title));
                Assert.IsNotNull(firstOrderDetails.Book.Author);
                Assert.That(firstOrderDetails.Book.Author.FirstName, Is.EqualTo(secondOrderDetails.Book.Author.FirstName));
                Assert.That(firstOrderDetails.Book.Author.LastName, Is.EqualTo(secondOrderDetails.Book.Author.LastName));
                Assert.That(firstOrderDetails.Book.Author.Email, Is.EqualTo(secondOrderDetails.Book.Author.Email));
                Assert.That(firstOrderDetails.Book.Author.PhoneNumber, Is.EqualTo(secondOrderDetails.Book.Author.PhoneNumber));
                Assert.That(firstOrderDetails.Book.Price, Is.EqualTo(secondOrderDetails.Book.Price));
                Assert.That(firstOrderDetails.Book.ISBN, Is.EqualTo(secondOrderDetails.Book.ISBN));
            }
        }
    }

    [Test]
    public async Task GetOrderForSummaryAsync_ShouldReturnCorrectOrder()
    {
        //Arrange
        IOrderRetrievalService service
            = new OrderRetrievalService(
                this._mockOrderDetailsRetrievalService!.Object,
                this._mockOrderHeaderRetrievalService!.Object,
                this._mockUnitOfWork!.Object);

        //Act
        OrderViewModel orderModel 
            = await service.GetOrderForSummaryAsync(this._existingOrder!.Id);

        //Assert
        Assert.That(orderModel.Id, Is.EqualTo(this._existingOrder!.Id));
        Assert.That(orderModel.OrderHeaderId, Is.EqualTo(this._existingOrder!.OrderHeaderId));
    }

    [Test]
    public void GetOrderForSummaryAsync_ShouldThrowOrderNotFoundException()
    {
        //Arrange
        IOrderRetrievalService service
            = new OrderRetrievalService(
                this._mockOrderDetailsRetrievalService!.Object,
                this._mockOrderHeaderRetrievalService!.Object,
                this._mockUnitOfWork!.Object);

        //Act & Assert
        Assert.ThrowsAsync<OrderNotFoundException>(async () =>
            await service.GetOrderForSummaryAsync(Guid.NewGuid()));
    }

    [Test]
    public async Task GetOrderForDetailsAsync_ShouldReturnCorrectOrder()
    {
        //Arrange
        IOrderRetrievalService service
            = new OrderRetrievalService(
                this._mockOrderDetailsRetrievalService!.Object,
                this._mockOrderHeaderRetrievalService!.Object,
                this._mockUnitOfWork!.Object);

        //Act
        DetailsOrderViewModel orderModel
            = await service.GetOrderForDetailsAsync(this._existingOrder!.Id);

        //Assert
        Assert.That(orderModel.Id, Is.EqualTo(this._existingOrder!.Id));
        Assert.That(orderModel.OrderHeaderId, Is.EqualTo(this._existingOrder!.OrderHeaderId));
    }

    [Test]
    public void GetOrderForDetailsAsync_ShouldThrowOrderNotFoundException()
    {
        //Arrange
        IOrderRetrievalService service
            = new OrderRetrievalService(
                this._mockOrderDetailsRetrievalService!.Object,
                this._mockOrderHeaderRetrievalService!.Object,
                this._mockUnitOfWork!.Object);

        //Act & Assert
        Assert.ThrowsAsync<OrderNotFoundException>(async () =>
            await service.GetOrderForDetailsAsync(Guid.NewGuid()));
    }

    [Test]
    public async Task GetOrderIdByOrderHeaderIdAsync_ShouldReturnCorrectOrderId()
    {
        //Arrange
        IOrderRetrievalService service
            = new OrderRetrievalService(
                this._mockOrderDetailsRetrievalService!.Object,
                this._mockOrderHeaderRetrievalService!.Object,
                this._mockUnitOfWork!.Object);

        //Act
        Guid orderId = await service.GetOrderIdByOrderHeaderIdAsync(this._existingOrderHeader!.Id);

        //Assert
        Assert.That(orderId, Is.EqualTo(this._existingOrder!.Id));
    }

    [Test]
    public void GetOrderIdByOrderHeaderIdAsync_ShouldThrowOrderNotFoundException()
    {
        //Arrange
        IOrderRetrievalService service
            = new OrderRetrievalService(
                this._mockOrderDetailsRetrievalService!.Object,
                this._mockOrderHeaderRetrievalService!.Object,
                this._mockUnitOfWork!.Object);

        //Act & Assert
        Assert.ThrowsAsync<OrderNotFoundException>(async () =>
                       await service.GetOrderIdByOrderHeaderIdAsync(Guid.NewGuid()));
    }
}