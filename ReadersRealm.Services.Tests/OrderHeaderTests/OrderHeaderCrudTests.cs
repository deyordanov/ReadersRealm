using Moq;
using ReadersRealm.Services.Data.OrderHeaderServices.Contracts;
using ReadersRealm.Services.Data.OrderHeaderServices;
using ReadersRealm.Web.ViewModels.OrderHeader;
using ReadersRealm.Common.Exceptions.OrderHeader;

namespace ReadersRealm.Services.Tests.OrderHeaderTests;

using ReadersRealm.Data.Models;
using ReadersRealm.Data.Repositories.Contracts;

public class OrderHeaderCrudTests
{
    private Mock<IUnitOfWork>? _mockUnitOfWork;

    private OrderHeader? _existingOrderHeader;

    [SetUp]
    public void SetUp()
    {
        this._mockUnitOfWork = new Mock<IUnitOfWork>();

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
        };

        this._mockUnitOfWork.Setup(uow => uow
                       .OrderHeaderRepository
                       .AddAsync(It.IsAny<OrderHeader>()))
            .Returns(Task.CompletedTask);

        this._mockUnitOfWork.Setup(uow => uow
                              .OrderHeaderRepository
                              .GetByIdAsync(this._existingOrderHeader!.Id))
            .ReturnsAsync(this._existingOrderHeader);
    }

    [Test]
    public async Task CreateOrderHeaderAsync_ShouldCreateOrderHeader()
    {
        //Arrange
        IOrderHeaderCrudService service
            = new OrderHeaderCrudService(this._mockUnitOfWork!.Object);

        OrderHeaderViewModel orderHeaderModel = new OrderHeaderViewModel()
        {
            FirstName = "FirstName",
            LastName = "LastName",
            PhoneNumber = "PhoneNumber",
            City = "City",
            PostalCode = "PostalCode",
            State = "State",
            StreetAddress = "StreetAddress",
            ApplicationUserId = this._existingOrderHeader!.ApplicationUserId,
            Carrier = this._existingOrderHeader!.Carrier,
            OrderDate = this._existingOrderHeader!.OrderDate,
            OrderStatus = this._existingOrderHeader!.OrderStatus,
            OrderTotal = this._existingOrderHeader!.OrderTotal,
            ShippingDate = this._existingOrderHeader!.ShippingDate,
            SessionId = this._existingOrderHeader!.SessionId,
            TrackingNumber = this._existingOrderHeader!.TrackingNumber,
            PaymentDueDate = this._existingOrderHeader!.PaymentDueDate,
            PaymentStatus = this._existingOrderHeader!.PaymentStatus,
            PaymentIntentId = this._existingOrderHeader!.PaymentIntentId,
            PaymentDate = this._existingOrderHeader!.PaymentDate,
        };

        //Act
        await service.CreateOrderHeaderAsync(orderHeaderModel);

        //Assert
        this._mockUnitOfWork.Verify(uow => uow
            .OrderHeaderRepository
            .AddAsync(It.Is<OrderHeader>(oh =>
                oh.ApplicationUserId == this._existingOrderHeader!.ApplicationUserId &&
                oh.Carrier == this._existingOrderHeader!.Carrier &&
                oh.OrderDate == this._existingOrderHeader!.OrderDate &&
                oh.OrderStatus == this._existingOrderHeader!.OrderStatus &&
                oh.OrderTotal == this._existingOrderHeader!.OrderTotal &&
                oh.ShippingDate == this._existingOrderHeader!.ShippingDate &&
                oh.SessionId == this._existingOrderHeader!.SessionId &&
                oh.TrackingNumber == this._existingOrderHeader!.TrackingNumber &&
                oh.PaymentDueDate == this._existingOrderHeader!.PaymentDueDate &&
                oh.PaymentStatus == this._existingOrderHeader!.PaymentStatus &&
                oh.PaymentIntentId == this._existingOrderHeader!.PaymentIntentId &&
                oh.PaymentDate == this._existingOrderHeader!.PaymentDate)));

        this._mockUnitOfWork.Verify(uow => uow.SaveAsync(), Times.Once());
    }

    [Test]
    public async Task UpdateOrderHeaderAsync_ShouldUpdateOrderHeaderCorrectly()
    {
        //Arrange
        IOrderHeaderCrudService service
            = new OrderHeaderCrudService(this._mockUnitOfWork!.Object);

        OrderHeaderViewModel orderHeaderModel = new OrderHeaderViewModel()
        {
            Id = this._existingOrderHeader!.Id,
            FirstName = "FirstName",
            LastName = "LastName",
            PhoneNumber = "PhoneNumber",
            City = "City",
            PostalCode = "PostalCode",
            State = "State",
            StreetAddress = "StreetAddress",
            ApplicationUserId = Guid.NewGuid(),
            Carrier = "UpdatedCarrier",
            OrderDate = DateTime.Now,
            OrderStatus = "UpdatedOrderStatus",
            OrderTotal = 200,
            ShippingDate = DateTime.Now,
            SessionId = "UpdatedSessionId",
            TrackingNumber = "UpdatedTrackingNumber",
            PaymentDueDate = DateOnly.FromDateTime(DateTime.Now),
            PaymentStatus = "UpdatedPaymentStatus",
            PaymentIntentId = "UpdatedPaymentIntentId",
            PaymentDate = DateTime.Now,
        };

        //Act
        await service.UpdateOrderHeaderAsync(orderHeaderModel);

        //Assert
        Assert.That(orderHeaderModel.ApplicationUserId, Is.EqualTo(this._existingOrderHeader!.ApplicationUserId));
        Assert.That(orderHeaderModel.Carrier, Is.EqualTo(this._existingOrderHeader!.Carrier));
        Assert.That(orderHeaderModel.OrderDate, Is.EqualTo(this._existingOrderHeader!.OrderDate));
        Assert.That(orderHeaderModel.OrderStatus, Is.EqualTo(this._existingOrderHeader!.OrderStatus));
        Assert.That(orderHeaderModel.OrderTotal, Is.EqualTo(this._existingOrderHeader!.OrderTotal));
        Assert.That(orderHeaderModel.ShippingDate, Is.EqualTo(this._existingOrderHeader!.ShippingDate));
        Assert.That(orderHeaderModel.SessionId, Is.EqualTo(this._existingOrderHeader!.SessionId));
        Assert.That(orderHeaderModel.TrackingNumber, Is.EqualTo(this._existingOrderHeader!.TrackingNumber));
        Assert.That(orderHeaderModel.PaymentDueDate, Is.EqualTo(this._existingOrderHeader!.PaymentDueDate));
        Assert.That(orderHeaderModel.PaymentStatus, Is.EqualTo(this._existingOrderHeader!.PaymentStatus));
        Assert.That(orderHeaderModel.PaymentIntentId, Is.EqualTo(this._existingOrderHeader!.PaymentIntentId));
        Assert.That(orderHeaderModel.PaymentDate, Is.EqualTo(this._existingOrderHeader!.PaymentDate));
    }

    [Test]
    public void UpdateOrderHeaderAsync_ShouldThrowOrderHeaderNotFoundException()
    {
        //Arrange
        IOrderHeaderCrudService service
            = new OrderHeaderCrudService(this._mockUnitOfWork!.Object);

        OrderHeaderViewModel orderHeaderModel = new OrderHeaderViewModel()
        {
            Id = Guid.NewGuid(),
            FirstName = "FirstName",
            LastName = "LastName",
            PhoneNumber = "PhoneNumber",
            City = "City",
            PostalCode = "PostalCode",
            State = "State",
            StreetAddress = "StreetAddress",
            ApplicationUserId = this._existingOrderHeader!.ApplicationUserId,
            Carrier = this._existingOrderHeader!.Carrier,
            OrderDate = this._existingOrderHeader!.OrderDate,
            OrderStatus = this._existingOrderHeader!.OrderStatus,
            OrderTotal = this._existingOrderHeader!.OrderTotal,
            ShippingDate = this._existingOrderHeader!.ShippingDate,
            SessionId = this._existingOrderHeader!.SessionId,
            TrackingNumber = this._existingOrderHeader!.TrackingNumber,
            PaymentDueDate = this._existingOrderHeader!.PaymentDueDate,
            PaymentStatus = this._existingOrderHeader!.PaymentStatus,
            PaymentIntentId = this._existingOrderHeader!.PaymentIntentId,
            PaymentDate = this._existingOrderHeader!.PaymentDate,
        };

        //Act
        Assert.ThrowsAsync<OrderHeaderNotFoundException>(async () =>
            await service.UpdateOrderHeaderAsync(orderHeaderModel));
    }

    [Test]
    public async Task UpdateOrderHeaderStatusAsync_ShouldUpdateOrderHeaderStatusCorrectly()
    {
        //Arrange
        IOrderHeaderCrudService service
            = new OrderHeaderCrudService(this._mockUnitOfWork!.Object);

        string updatedOrderStatus = "UpdatedOrderStatus";
        string updatedPaymentStatus = "UpdatedPaymentStatus";

        //Act
        await service.UpdateOrderHeaderStatusAsync(
            this._existingOrderHeader!.Id, 
            updatedOrderStatus,
            updatedPaymentStatus);

        //Assert
        Assert.That(this._existingOrderHeader!.OrderStatus, Is.EqualTo(updatedOrderStatus));
        Assert.That(this._existingOrderHeader!.PaymentStatus, Is.EqualTo(updatedPaymentStatus));

        this._mockUnitOfWork.Verify(uow => uow.SaveAsync(), Times.Once());
    }

    [Test]
    public void UpdateOrderHeaderStatusAsync_ShouldThrowOrderHeaderNotFoundException()
    {
        //Arrange
        IOrderHeaderCrudService service
            = new OrderHeaderCrudService(this._mockUnitOfWork!.Object);

        string updatedOrderStatus = "UpdatedOrderStatus";
        string updatedPaymentStatus = "UpdatedPaymentStatus";

        //Act & Assert
        Assert.ThrowsAsync<OrderHeaderNotFoundException>(async () =>
                       await service.UpdateOrderHeaderStatusAsync(
                           Guid.NewGuid(), 
                           updatedOrderStatus, 
                           updatedPaymentStatus));
    }

    [Test]
    public async Task UpdateOrderHeaderPaymentIntentIdAsync_ShouldUpdateOrderHeaderPaymentIntentIdCorrectly()
    {
        //Arrange
        IOrderHeaderCrudService service
            = new OrderHeaderCrudService(this._mockUnitOfWork!.Object);

        string updatedSessionId = "UpdatedSessionId";
        string updatedPaymentIntentId = "UpdatedPaymentIntentId";

        //Act
        await service.UpdateOrderHeaderPaymentIntentIdAsync(
            this._existingOrderHeader!.Id,
            updatedSessionId,
            updatedPaymentIntentId);

        //Assert
        Assert.That(this._existingOrderHeader!.SessionId, Is.EqualTo(updatedSessionId));
        Assert.That(this._existingOrderHeader!.PaymentIntentId, Is.EqualTo(updatedPaymentIntentId));

        this._mockUnitOfWork.Verify(uow => uow.SaveAsync(), Times.Once());
    }

    [Test]
    public void UpdateOrderHeaderPaymentIntentIdAsync_ShouldThrowOrderHeaderNotFoundException()
    {
        //Arrange
        IOrderHeaderCrudService service
            = new OrderHeaderCrudService(this._mockUnitOfWork!.Object);

        string updatedSessionId = "UpdatedSessionId";
        string updatedPaymentIntentId = "UpdatedPaymentIntentId";

        //Act & Assert
        Assert.ThrowsAsync<OrderHeaderNotFoundException>(async () =>
                       await service.UpdateOrderHeaderPaymentIntentIdAsync(
                           Guid.NewGuid(),
                           updatedSessionId,
                           updatedPaymentIntentId));
    }

    [Test]
    public async Task DeleteOrderHeaderAsync_ShouldDeleteOrderHeader()
    {
        //Arrange
        IOrderHeaderCrudService service
            = new OrderHeaderCrudService(this._mockUnitOfWork!.Object);

        OrderHeaderViewModel companyModel = new OrderHeaderViewModel()
        {
            Id = this._existingOrderHeader!.Id,
            FirstName = "FirstName",
            LastName = "LastName",
            PhoneNumber = "PhoneNumber",
            City = "City",
            PostalCode = "PostalCode",
            State = "State",
            StreetAddress = "StreetAddress",
            ApplicationUserId = this._existingOrderHeader!.ApplicationUserId,
            Carrier = this._existingOrderHeader!.Carrier,
            OrderDate = this._existingOrderHeader!.OrderDate,
            OrderStatus = this._existingOrderHeader!.OrderStatus,
            OrderTotal = this._existingOrderHeader!.OrderTotal,
            ShippingDate = this._existingOrderHeader!.ShippingDate,
            SessionId = this._existingOrderHeader!.SessionId,
            TrackingNumber = this._existingOrderHeader!.TrackingNumber,
            PaymentDueDate = this._existingOrderHeader!.PaymentDueDate,
            PaymentStatus = this._existingOrderHeader!.PaymentStatus,
            PaymentIntentId = this._existingOrderHeader!.PaymentIntentId,
            PaymentDate = this._existingOrderHeader!.PaymentDate,
        };

        //Act
        await service.DeleteOrderHeaderAsync(companyModel);

        //Assert
        this._mockUnitOfWork.Verify(uow => uow
            .OrderHeaderRepository
            .Delete(It.Is<OrderHeader>(oh =>
                oh.ApplicationUserId == this._existingOrderHeader!.ApplicationUserId &&
                oh.Carrier == this._existingOrderHeader!.Carrier &&
                oh.OrderDate == this._existingOrderHeader!.OrderDate &&
                oh.OrderStatus == this._existingOrderHeader!.OrderStatus &&
                oh.OrderTotal == this._existingOrderHeader!.OrderTotal &&
                oh.ShippingDate == this._existingOrderHeader!.ShippingDate &&
                oh.SessionId == this._existingOrderHeader!.SessionId &&
                oh.TrackingNumber == this._existingOrderHeader!.TrackingNumber &&
                oh.PaymentDueDate == this._existingOrderHeader!.PaymentDueDate &&
                oh.PaymentStatus == this._existingOrderHeader!.PaymentStatus &&
                oh.PaymentIntentId == this._existingOrderHeader!.PaymentIntentId &&
                oh.PaymentDate == this._existingOrderHeader!.PaymentDate)));

        this._mockUnitOfWork.Verify(uow => uow
            .OrderHeaderRepository
            .Delete(It.IsAny<OrderHeader>()), Times.Once());

        this._mockUnitOfWork.Verify(uow => uow.SaveAsync(), Times.Once());
    }

    [Test]
    public void DeleteOrderHeaderAsync_ShouldThrowOrderHeaderNotFoundException()
    {
        //Arrange
        IOrderHeaderCrudService service
            = new OrderHeaderCrudService(this._mockUnitOfWork!.Object);

        OrderHeaderViewModel companyModel = new OrderHeaderViewModel()
        {
            Id = Guid.NewGuid(),
            FirstName = "FirstName",
            LastName = "LastName",
            PhoneNumber = "PhoneNumber",
            City = "City",
            PostalCode = "PostalCode",
            State = "State",
            StreetAddress = "StreetAddress",
            ApplicationUserId = this._existingOrderHeader!.ApplicationUserId,
            Carrier = this._existingOrderHeader!.Carrier,
            OrderDate = this._existingOrderHeader!.OrderDate,
            OrderStatus = this._existingOrderHeader!.OrderStatus,
            OrderTotal = this._existingOrderHeader!.OrderTotal,
            ShippingDate = this._existingOrderHeader!.ShippingDate,
            SessionId = this._existingOrderHeader!.SessionId,
            TrackingNumber = this._existingOrderHeader!.TrackingNumber,
            PaymentDueDate = this._existingOrderHeader!.PaymentDueDate,
            PaymentStatus = this._existingOrderHeader!.PaymentStatus,
            PaymentIntentId = this._existingOrderHeader!.PaymentIntentId,
            PaymentDate = this._existingOrderHeader!.PaymentDate,
        };

        //Act & Assert
        Assert.ThrowsAsync<OrderHeaderNotFoundException>(async () =>
            await service.DeleteOrderHeaderAsync(companyModel));
    }
}