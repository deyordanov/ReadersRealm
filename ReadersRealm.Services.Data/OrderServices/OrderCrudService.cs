namespace ReadersRealm.Services.Data.OrderServices;

using ApplicationUserServices.Contracts;
using Contracts;
using OrderHeaderServices.Contracts;
using Web.ViewModels.ApplicationUser;
using Web.ViewModels.Order;

public class OrderCrudService : IOrderCrudService
{
    private readonly IApplicationUserCrudService _applicationUserCrudService;
    private readonly IOrderHeaderCrudService _orderHeaderCrudService;

    public OrderCrudService(
        IApplicationUserCrudService applicationUserCrudService, 
        IOrderHeaderCrudService orderHeaderCrudService)
    {
        this._applicationUserCrudService = applicationUserCrudService;
        this._orderHeaderCrudService = orderHeaderCrudService;
    }

    public async Task UpdateOrderAsync(DetailsOrderViewModel orderModel)
    {
        OrderApplicationUserViewModel applicationUserModel = new OrderApplicationUserViewModel()
        {
            Id = orderModel.OrderHeader.ApplicationUserId,
            FirstName = orderModel.OrderHeader.FirstName,
            LastName = orderModel.OrderHeader.LastName,
            PhoneNumber = orderModel.OrderHeader.PhoneNumber,
            City = orderModel.OrderHeader.City,
            PostalCode = orderModel.OrderHeader.PostalCode,
            State = orderModel.OrderHeader.State,
            StreetAddress = orderModel.OrderHeader.StreetAddress,
        };

        await this
            ._applicationUserCrudService
            .UpdateApplicationUserAsync(applicationUserModel);

        await this
            ._orderHeaderCrudService
            .UpdateOrderHeaderAsync(orderModel.OrderHeader);
    }
}