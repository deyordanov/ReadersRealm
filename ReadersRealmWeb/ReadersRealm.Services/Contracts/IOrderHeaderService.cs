namespace ReadersRealm.Services.Contracts;

using ViewModels.OrderHeader;

public interface IOrderHeaderService
{
    public Task<OrderHeaderViewModel> GetByIdAsyncWithNavPropertiesAsync(Guid id);
    Task<Guid> CreateOrderHeaderAsync(OrderHeaderViewModel orderHeaderModel);
    Task UpdateOrderHeaderStatusAsync(Guid id, string orderStatus, string? paymentStatus);
    Task UpdateOrderHeaderPaymentIntentIdAsync(Guid id, string sessionId, string paymentIntentId);
}