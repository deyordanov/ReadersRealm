namespace ReadersRealm.Services.Data.OrderDetailsServices.Contracts;

using Web.ViewModels.OrderDetails;

public interface IOrderDetailsCrudService
{
    Task CreateOrderDetailsAsync(OrderDetailsViewModel orderDetailsModel);
    Task DeleteOrderDetailsRangeByOrderHeaderIdAsync(Guid orderHeaderId);
}