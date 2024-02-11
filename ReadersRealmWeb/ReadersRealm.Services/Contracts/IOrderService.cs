namespace ReadersRealm.Services.Contracts;

using Common;
using ViewModels.Order;

public interface IOrderService
{
    Task<PaginatedList<AllOrdersViewModel>> GetAllAsync(int pageIndex, int pageSize, string? searchTerm);
    Task<PaginatedList<AllOrdersViewModel>> GetAllByUserIdAsync(int pageIndex, int pageSize, string? searchTerm, string userId);
    Task<OrderViewModel> GetOrderForSummaryAsync(Guid id);
    Task<DetailsOrderViewModel> GetOrderForDetailsAsync(Guid id);
    Task<Guid> GetOrderIdByOrderHeaderIdAsync(Guid orderHeaderId);
    Task UpdateOrderAsync(DetailsOrderViewModel orderModel);
}