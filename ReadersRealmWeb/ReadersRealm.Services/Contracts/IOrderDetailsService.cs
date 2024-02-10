namespace ReadersRealm.Services.Contracts;

using DataTransferObjects.OrderDetails;
using ReadersRealm.ViewModels.Book;
using ViewModels.OrderDetails;

public interface IOrderDetailsService
{
    Task CreateOrderDetailsAsync(OrderDetailsViewModel orderDetailsModel);
    Task<IEnumerable<OrderDetailsViewModel>> GetAllByOrderHeaderAsync(Guid orderHeaderId);
    Task<IEnumerable<OrderDetailsDto>> GetAllByOrderHeaderAsDtosAsync(Guid orderHeaderId);
    Task<IEnumerable<OrderDetailsReceiptDto>> GetAllOrderDetailsForReceiptAsDtosAsync(Guid orderHeaderId);
    Task DeleteOrderDetailsRangeByOrderHeaderIdAsync(Guid orderHeaderId);
}