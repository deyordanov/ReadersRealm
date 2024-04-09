namespace ReadersRealm.Services.Tests.OrderHeaderTests;

using System.Linq.Expressions;
using Common.Exceptions.OrderHeader;
using Data.Models.OrderHeader;
using Data.OrderDetailsServices.Contracts;
using Data.OrderHeaderServices;
using Moq;
using ReadersRealm.Data.Models;
using ReadersRealm.Data.Repositories.Contracts;
using Web.ViewModels.OrderHeader;

public class OrderHeaderRetrievalTests
{
    private Mock<IUnitOfWork>? _mockUnitOfWork;
    private Mock<IOrderDetailsRetrievalService>? _mockOrderDetailsRetrievalService;

    private OrderHeader? _existingOrderHeader;
    private List<OrderHeader>? _allOrderHeaders;

    [SetUp]
    public void Setup()
    {
        this._mockUnitOfWork = new Mock<IUnitOfWork>();
        this._mockOrderDetailsRetrievalService = new Mock<IOrderDetailsRetrievalService>();

        this._existingOrderHeader = new OrderHeader()
        {
            ApplicationUserId = Guid.NewGuid(),
            Carrier = "Carrier",
            OrderDate = DateTime.Now,
            OrderStatus = "OrderStatus",
            OrderTotal = 100,
            ShippingDate = DateTime.Now,
            SessionId = "SessionId",
            TrackingNumber = "TrackingNumber",
            PaymentDueDate = DateOnly.FromDateTime(DateTime.Now),
            PaymentStatus = "PaymentStatus",
            PaymentIntentId = "PaymentIntentId",
            PaymentDate = DateTime.Now,
            ApplicationUser = new ApplicationUser()
            {
                Id = Guid.NewGuid(),
                City = "City",
                FirstName = "FirstName",
                LastName = "LastName",
                PhoneNumber = "PhoneNumber",
                PostalCode = "PostalCode",
                State = "State",
                StreetAddress = "StreetAddress",
                UserName = "Email"
            }
        };

        this._allOrderHeaders = new List<OrderHeader>()
        {
            this._existingOrderHeader,
            new OrderHeader()
            {
                ApplicationUserId = Guid.NewGuid(),
                Carrier = "Carrier1",
                OrderDate = DateTime.Now,
                OrderStatus = "OrderStatus1",
                OrderTotal = 100,
                ShippingDate = DateTime.Now,
                SessionId = "SessionId1",
                TrackingNumber = "TrackingNumber1",
                PaymentDueDate = DateOnly.FromDateTime(DateTime.Now),
                PaymentStatus = "PaymentStatus1",
                PaymentIntentId = "PaymentIntentId1",
                PaymentDate = DateTime.Now,
                ApplicationUser = new ApplicationUser()
                {
                    Id = Guid.NewGuid(),
                    City = "City1",
                    FirstName = "FirstName1",
                    LastName = "LastName1",
                    PhoneNumber = "PhoneNumber1",
                    PostalCode = "PostalCode1",
                    State = "State1",
                    StreetAddress = "StreetAddress1",
                    UserName = "Email1"
                }
            },
            new OrderHeader()
            {
                ApplicationUserId = Guid.NewGuid(),
                Carrier = "Carrier2",
                OrderDate = DateTime.Now,
                OrderStatus = "OrderStatus2",
                OrderTotal = 100,
                ShippingDate = DateTime.Now,
                SessionId = "SessionId2",
                TrackingNumber = "TrackingNumber2",
                PaymentDueDate = DateOnly.FromDateTime(DateTime.Now),
                PaymentStatus = "PaymentStatus2",
                PaymentIntentId = "PaymentIntentId2",
                PaymentDate = DateTime.Now,
                ApplicationUser = new ApplicationUser()
                {
                    Id = Guid.NewGuid(),
                    City = "City2",
                    FirstName = "FirstName2",
                    LastName = "LastName2",
                    PhoneNumber = "PhoneNumber2",
                    PostalCode = "PostalCode2",
                    State = "State2",
                    StreetAddress = "StreetAddress2",
                    UserName = "Email2"
                }
            },
            new OrderHeader()
            {
                ApplicationUserId = Guid.NewGuid(),
                Carrier = "Carrier3",
                OrderDate = DateTime.Now,
                OrderStatus = "OrderStatus3",
                OrderTotal = 100,
                ShippingDate = DateTime.Now,
                SessionId = "SessionId3",
                TrackingNumber = "TrackingNumber3",
                PaymentDueDate = DateOnly.FromDateTime(DateTime.Now),
                PaymentStatus = "PaymentStatus3",
                PaymentIntentId = "PaymentIntentId3",
                PaymentDate = DateTime.Now,
                ApplicationUser = new ApplicationUser()
                {
                    Id = Guid.NewGuid(),
                    City = "City3",
                    FirstName = "FirstName3",
                    LastName = "LastName3",
                    PhoneNumber = "PhoneNumber3",
                    PostalCode = "PostalCode3",
                    State = "State3",
                    StreetAddress = "StreetAddress3",
                    UserName = "Email3"
                }
            },
        };

        this._mockUnitOfWork!.Setup(uow => uow
                .OrderHeaderRepository
                .GetByIdWithNavPropertiesAsync(this._existingOrderHeader!.Id, 
                    It.IsAny<string>()))
            .ReturnsAsync(this._existingOrderHeader);

        this._mockUnitOfWork!.Setup(uow => uow
                .OrderHeaderRepository
                .GetAsync(It.IsAny<Expression<Func<OrderHeader, bool>>>(),
                    It.IsAny<Func<IQueryable<OrderHeader>,
                        IOrderedQueryable<OrderHeader>>>(),
                    It.IsAny<string>()))
            .ReturnsAsync(this._allOrderHeaders);

        this._mockUnitOfWork!.Setup(uow => uow
                .OrderHeaderRepository
                .GetByIdAsync(this._existingOrderHeader!.Id))
            .ReturnsAsync(this._existingOrderHeader);
    }

    [Test]
    public async Task GetByIdAsyncWithNavPropertiesAsync_ShouldReturnCorrectOrderHeader()
    {
        // Arrange
        OrderHeaderRetrievalService orderHeaderRetrievalService 
            = new OrderHeaderRetrievalService(
                       this._mockUnitOfWork!.Object,
                       this._mockOrderDetailsRetrievalService!.Object);

        // Act
        OrderHeaderViewModel orderHeaderModel 
            = await orderHeaderRetrievalService.GetByIdAsyncWithNavPropertiesAsync(this._existingOrderHeader!.Id);

        // Assert
        Assert.NotNull(orderHeaderModel);
        Assert.That(this._existingOrderHeader!.ApplicationUserId, Is.EqualTo(orderHeaderModel.ApplicationUserId));
        Assert.That(this._existingOrderHeader!.Carrier, Is.EqualTo(orderHeaderModel.Carrier));
        Assert.That(this._existingOrderHeader!.OrderDate, Is.EqualTo(orderHeaderModel.OrderDate));
        Assert.That(this._existingOrderHeader!.OrderStatus, Is.EqualTo(orderHeaderModel.OrderStatus));
        Assert.That(this._existingOrderHeader!.OrderTotal, Is.EqualTo(orderHeaderModel.OrderTotal));
        Assert.That(this._existingOrderHeader!.ShippingDate, Is.EqualTo(orderHeaderModel.ShippingDate));
        Assert.That(this._existingOrderHeader!.SessionId, Is.EqualTo(orderHeaderModel.SessionId));
        Assert.That(this._existingOrderHeader!.TrackingNumber, Is.EqualTo(orderHeaderModel.TrackingNumber));
        Assert.That(this._existingOrderHeader!.PaymentDueDate, Is.EqualTo(orderHeaderModel.PaymentDueDate));
        Assert.That(this._existingOrderHeader!.PaymentStatus, Is.EqualTo(orderHeaderModel.PaymentStatus));
        Assert.That(this._existingOrderHeader!.PaymentIntentId, Is.EqualTo(orderHeaderModel.PaymentIntentId));
        Assert.That(this._existingOrderHeader!.PaymentDate, Is.EqualTo(orderHeaderModel.PaymentDate));
        Assert.That(this._existingOrderHeader!.ApplicationUser.City, Is.EqualTo(orderHeaderModel.City));
        Assert.That(this._existingOrderHeader!.ApplicationUser.FirstName, Is.EqualTo(orderHeaderModel.FirstName));
        Assert.That(this._existingOrderHeader!.ApplicationUser.LastName, Is.EqualTo(orderHeaderModel.LastName));
        Assert.That(this._existingOrderHeader!.ApplicationUser.PhoneNumber, Is.EqualTo(orderHeaderModel.PhoneNumber));
        Assert.That(this._existingOrderHeader!.ApplicationUser.PostalCode, Is.EqualTo(orderHeaderModel.PostalCode));
        Assert.That(this._existingOrderHeader!.ApplicationUser.State, Is.EqualTo(orderHeaderModel.State));
        Assert.That(this._existingOrderHeader!.ApplicationUser.StreetAddress, Is.EqualTo(orderHeaderModel.StreetAddress));
    }

    [Test]
    public void GetByIdAsyncWithNavPropertiesAsync_ShouldThrowOrderHeaderNotFoundException()
    {
        // Arrange
        OrderHeaderRetrievalService orderHeaderRetrievalService
            = new OrderHeaderRetrievalService(
                this._mockUnitOfWork!.Object,
                this._mockOrderDetailsRetrievalService!.Object);

        // Act
        Assert.ThrowsAsync<OrderHeaderNotFoundException>(async () => 
            await orderHeaderRetrievalService.GetByIdAsyncWithNavPropertiesAsync(Guid.NewGuid()));
    }

    [Test]
    public async Task GetByApplicationUserIdAndOrderStatusAsync_ShouldReturnCorrectOrderHeader()
    {
        // Arrange
        OrderHeaderRetrievalService orderHeaderRetrievalService
            = new OrderHeaderRetrievalService(
                this._mockUnitOfWork!.Object,
                this._mockOrderDetailsRetrievalService!.Object);

        //Act
        OrderHeaderViewModel? orderHeaderModel
            = await orderHeaderRetrievalService
                .GetByApplicationUserIdAndOrderStatusAsync(
                               this._existingOrderHeader!.ApplicationUserId, 
                               this._existingOrderHeader!.OrderStatus!);

        OrderHeader orderHeader = this._allOrderHeaders!
            .FirstOrDefault(o => o.Id == orderHeaderModel!.Id)!;

        //Assert
        Assert.NotNull(orderHeaderModel);


        Assert.That(orderHeader.Id, Is.EqualTo(orderHeaderModel!.Id));
        Assert.That(orderHeader.ApplicationUserId, Is.EqualTo(orderHeaderModel!.ApplicationUserId));
        Assert.That(orderHeader.Carrier, Is.EqualTo(orderHeaderModel.Carrier));
        Assert.That(orderHeader.OrderDate, Is.EqualTo(orderHeaderModel.OrderDate));
        Assert.That(orderHeader.OrderStatus, Is.EqualTo(orderHeaderModel.OrderStatus));
        Assert.That(orderHeader.OrderTotal, Is.EqualTo(orderHeaderModel.OrderTotal));
        Assert.That(orderHeader.ShippingDate, Is.EqualTo(orderHeaderModel.ShippingDate));
        Assert.That(orderHeader.SessionId, Is.EqualTo(orderHeaderModel.SessionId));
        Assert.That(orderHeader.TrackingNumber, Is.EqualTo(orderHeaderModel.TrackingNumber));
        Assert.That(orderHeader.PaymentDueDate, Is.EqualTo(orderHeaderModel.PaymentDueDate));
        Assert.That(orderHeader.PaymentStatus, Is.EqualTo(orderHeaderModel.PaymentStatus));
        Assert.That(orderHeader.PaymentIntentId, Is.EqualTo(orderHeaderModel.PaymentIntentId));
        Assert.That(orderHeader.PaymentDate, Is.EqualTo(orderHeaderModel.PaymentDate));
    }

    [Test]
    public async Task GetOrderHeaderForReceiptAsync_ShouldReturnCorrectOrderHeader()
    {
        // Arrange
        OrderHeaderRetrievalService orderHeaderRetrievalService
            = new OrderHeaderRetrievalService(
                this._mockUnitOfWork!.Object, 
                this._mockOrderDetailsRetrievalService!.Object);

        // Act
        OrderHeaderReceiptDto orderHeaderModel
            = await orderHeaderRetrievalService.GetOrderHeaderForReceiptAsync(this._existingOrderHeader!.Id);

        // Assert
        Assert.That(this._existingOrderHeader!.PaymentStatus, Is.EqualTo(orderHeaderModel.PaymentStatus));
        Assert.That(this._existingOrderHeader!.OrderTotal, Is.EqualTo(orderHeaderModel.OrderTotal));
        Assert.That(this._existingOrderHeader!.PaymentIntentId, Is.EqualTo(orderHeaderModel.PaymentIntentId));
        Assert.That(this._existingOrderHeader!.OrderDate, Is.EqualTo(orderHeaderModel.OrderDate));
        Assert.That(this._existingOrderHeader!.PaymentDate, Is.EqualTo(orderHeaderModel.PaymentDate));
    }

    [Test]
    public void GetOrderHeaderForReceiptAsync_ShouldThrowOrderHeaderNotFoundException()
    {
        // Arrange
        OrderHeaderRetrievalService orderHeaderRetrievalService
            = new OrderHeaderRetrievalService(
                               this._mockUnitOfWork!.Object,
                                              this._mockOrderDetailsRetrievalService!.Object);

        // Act
        Assert.ThrowsAsync<OrderHeaderNotFoundException>(async () =>
                       await orderHeaderRetrievalService.GetOrderHeaderForReceiptAsync(Guid.NewGuid()));
    }
}