namespace ReadersRealm.Services.Data.OrderHeaderServices.Contracts;

using Models.OrderHeader;
using Web.ViewModels.OrderHeader;

public interface IOrderHeaderRetrievalService
{
    Task<OrderHeaderViewModel> GetByIdAsyncWithNavPropertiesAsync(Guid id);
    Task<OrderHeaderViewModel?> GetByApplicationUserIdAndOrderStatusAsync(string applicationUserId, string orderStatus);
    Task<OrderHeaderReceiptDto> GetOrderHeaderForReceiptAsync(Guid id);
}