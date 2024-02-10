namespace ReadersRealm.Services.Contracts;

using DataTransferObjects.OrderHeader;
using ViewModels.OrderHeader;

public interface IOrderHeaderService
{
    Task<OrderHeaderViewModel> GetByIdAsyncWithNavPropertiesAsync(Guid id);
    Task<OrderHeaderViewModel?> GetByApplicationUserIdAndOrderStatusAsync(string applicationUserId, string orderStatus);
    Task<OrderHeaderReceiptDto> GetOrderHeaderForReceiptAsync(Guid id);
    Task<Guid> CreateOrderHeaderAsync(OrderHeaderViewModel orderHeaderModel);
    Task UpdateOrderHeaderAsync(OrderHeaderViewModel orderHeaderModel);
    Task UpdateOrderHeaderStatusAsync(Guid id, string orderStatus, string? paymentStatus);
    Task UpdateOrderHeaderPaymentIntentIdAsync(Guid id, string sessionId, string paymentIntentId);
    Task DeleteOrderHeaderAsync(OrderHeaderViewModel orderHeaderModel);
}