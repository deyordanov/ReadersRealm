namespace ReadersRealm.Services.Contracts;

using ReadersRealm.ViewModels.Book;
using ViewModels.OrderHeader;

public interface IOrderHeaderService
{
    public Task<OrderHeaderViewModel> GetByIdAsyncWithNavPropertiesAsync(Guid id);
    Task<Guid> CreateOrderHeaderAsync(OrderHeaderViewModel orderHeaderModel);
}