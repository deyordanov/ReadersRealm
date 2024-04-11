namespace ReadersRealm.Services.Tests.OrderTests;

using Data.ApplicationUserServices.Contracts;
using Data.OrderHeaderServices.Contracts;
using Data.OrderServices;
using Moq;
using Web.ViewModels.ApplicationUser;
using Web.ViewModels.Order;
using Web.ViewModels.OrderHeader;

[TestFixture]
public class OrderCrudTests
{
    private Mock<IApplicationUserCrudService>? _mockApplicationUserCrudService;
    private Mock<IOrderHeaderCrudService>? _mockOrderHeaderCrudService;

    private OrderApplicationUserViewModel? _existingApplicationUser;
    private OrderHeaderViewModel? _existingOrderHeader;

    [SetUp]
    public void Setup()
    {
        this._mockApplicationUserCrudService = new Mock<IApplicationUserCrudService>();
        this._mockOrderHeaderCrudService = new Mock<IOrderHeaderCrudService>();

        this._existingApplicationUser = new OrderApplicationUserViewModel()
        {
            Id = Guid.NewGuid(),
            FirstName = "FirstName",
            LastName = "LastName",
            PhoneNumber = "PhoneNumber",
            City = "City",
            PostalCode = "PostalCode",
            State = "State",
            StreetAddress = "StreetAddress",
        };

        this._existingOrderHeader = new OrderHeaderViewModel()
        {
            ApplicationUserId = this._existingApplicationUser.Id,
            FirstName = this._existingApplicationUser.FirstName,
            LastName = this._existingApplicationUser.LastName,
            PhoneNumber = this._existingApplicationUser.PhoneNumber,
            City = this._existingApplicationUser.City,
            PostalCode = this._existingApplicationUser.PostalCode,
            State = this._existingApplicationUser.State,
            StreetAddress = this._existingApplicationUser.StreetAddress,
        };

        this._mockApplicationUserCrudService.Setup(aucs => aucs
                .UpdateApplicationUserAsync(It.IsAny<OrderApplicationUserViewModel>()))
            .Returns(Task.CompletedTask);

        this._mockOrderHeaderCrudService.Setup(ohcs => ohcs
            .UpdateOrderHeaderAsync(It.IsAny<OrderHeaderViewModel>()))
            .Returns(Task.CompletedTask);
    }

    [Test]
    public async Task UpdateOrderAsync_ShouldUpdateOrderCorrectly()
    {
        //Arrange
        OrderCrudService service 
            = new OrderCrudService(
                       this._mockApplicationUserCrudService!.Object,
                       this._mockOrderHeaderCrudService!.Object);


        DetailsOrderViewModel orderModel = new DetailsOrderViewModel()
        {
            OrderHeader = new OrderHeaderViewModel()
            {
                Id = this._existingOrderHeader!.Id,
                ApplicationUserId = this._existingApplicationUser!.Id,
                FirstName = "UpdatedFirstName",
                LastName = "UpdatedLastName",
                PhoneNumber = "UpdatedPhoneNumber",
                City = "UpdatedCity",
                PostalCode = "UpdatedPostalCode",
                State = "UpdatedState",
                StreetAddress = "UpdatedStreetAddress",
            },
        };

        //Act
        await service.UpdateOrderAsync(orderModel);

        //Assert
        this._mockApplicationUserCrudService.Verify(aucs => aucs
                .UpdateApplicationUserAsync(It.IsAny<OrderApplicationUserViewModel>()), Times.Once);

        this._mockOrderHeaderCrudService.Verify(ohcs => ohcs
                .UpdateOrderHeaderAsync(It.IsAny<OrderHeaderViewModel>()), Times.Once);
    }
}