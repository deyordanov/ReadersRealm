namespace ReadersRealm.Services.Data.Contracts;

using ReadersRealm.DataTransferObjects.OrderDetails;
using ReadersRealm.ViewModels.OrderDetails;

public interface IOrderDetailsService
{
    Task CreateOrderDetailsAsync(OrderDetailsViewModel orderDetailsModel);
    Task<IEnumerable<OrderDetailsViewModel>> GetAllByOrderHeaderIdAsync(Guid orderHeaderId);
    Task<IEnumerable<OrderDetailsReceiptDto>> GetAllOrderDetailsForReceiptAsDtosAsync(Guid orderHeaderId);
    Task DeleteOrderDetailsRangeByOrderHeaderIdAsync(Guid orderHeaderId);
}