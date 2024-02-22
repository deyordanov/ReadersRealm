namespace ReadersRealm.Services.Data.OrderHeaderServices.Contracts;

using Web.ViewModels.OrderHeader;

public interface IOrderHeaderCrudService
{
    Task<(Guid orderHeaderId, Guid orderId)> CreateOrderHeaderAsync(OrderHeaderViewModel orderHeaderModel);
    Task UpdateOrderHeaderAsync(OrderHeaderViewModel orderHeaderModel);
    Task UpdateOrderHeaderStatusAsync(Guid id, string orderStatus, string? paymentStatus);
    Task UpdateOrderHeaderPaymentIntentIdAsync(Guid id, string sessionId, string paymentIntentId);
    Task DeleteOrderHeaderAsync(OrderHeaderViewModel orderHeaderModel);
}