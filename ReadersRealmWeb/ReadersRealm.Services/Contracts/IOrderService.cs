namespace ReadersRealm.Services.Contracts;

using Common;
using ViewModels.Book;
using ViewModels.Order;

public interface IOrderService
{
    Task<PaginatedList<AllOrdersViewModel>> GetAllAsync(int pageIndex, int pageSize, string? searchTerm);
    Task<PaginatedList<AllOrdersViewModel>> GetAllByUserIdAsync(int pageIndex, int pageSize, string? searchTerm, string userId);
    Task<DetailsOrderViewModel> GetOrderForDetailsAsync(Guid id);
    Task UpdateOrderAsync(DetailsOrderViewModel orderModel);
}