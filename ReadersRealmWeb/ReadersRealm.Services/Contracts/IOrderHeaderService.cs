namespace ReadersRealm.Services.Contracts;

using ViewModels.OrderHeader;

public interface IOrderHeaderService
{
    public Task<OrderHeaderViewModel> GetByIdAsyncWithNavPropertiesAsync(Guid id);
    Task<Guid> CreateOrderHeaderAsync(OrderHeaderViewModel orderHeaderModel);
}