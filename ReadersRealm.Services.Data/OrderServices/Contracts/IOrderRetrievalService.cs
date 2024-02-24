namespace ReadersRealm.Services.Data.OrderServices.Contracts;

using Common;
using Web.ViewModels.Order;

public interface IOrderRetrievalService
{
    Task<PaginatedList<AllOrdersViewModel>> GetAllAsync(int pageIndex, int pageSize, string? searchTerm);
    Task<PaginatedList<AllOrdersViewModel>> GetAllByUserIdAsync(int pageIndex, int pageSize, string? searchTerm, Guid userId);
    Task<OrderViewModel> GetOrderForSummaryAsync(Guid id);
    Task<DetailsOrderViewModel> GetOrderForDetailsAsync(Guid id);
    Task<Guid> GetOrderIdByOrderHeaderIdAsync(Guid orderHeaderId);
}