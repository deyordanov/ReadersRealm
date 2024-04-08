namespace ReadersRealm.Services.Data.OrderServices;

using ApplicationUserServices.Contracts;
using Contracts;
using OrderHeaderServices.Contracts;
using Web.ViewModels.ApplicationUser;
using Web.ViewModels.Order;

public class OrderCrudService(
    IApplicationUserCrudService applicationUserCrudService,
    IOrderHeaderCrudService orderHeaderCrudService)
    : IOrderCrudService
{
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

        await applicationUserCrudService
            .UpdateApplicationUserAsync(applicationUserModel);

        await orderHeaderCrudService
            .UpdateOrderHeaderAsync(orderModel.OrderHeader);
    }
}