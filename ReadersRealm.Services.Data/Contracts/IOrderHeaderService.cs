﻿namespace ReadersRealm.Services.Data.Contracts;

using ReadersRealm.DataTransferObjects.OrderHeader;
using ReadersRealm.ViewModels.OrderHeader;

public interface IOrderHeaderService
{
    Task<OrderHeaderViewModel> GetByIdAsyncWithNavPropertiesAsync(Guid id);
    Task<OrderHeaderViewModel?> GetByApplicationUserIdAndOrderStatusAsync(string applicationUserId, string orderStatus);
    Task<OrderHeaderReceiptDto> GetOrderHeaderForReceiptAsync(Guid id);
    Task<(Guid orderHeaderId, Guid orderId)> CreateOrderHeaderAsync(OrderHeaderViewModel orderHeaderModel);
    Task UpdateOrderHeaderAsync(OrderHeaderViewModel orderHeaderModel);
    Task UpdateOrderHeaderStatusAsync(Guid id, string orderStatus, string? paymentStatus);
    Task UpdateOrderHeaderPaymentIntentIdAsync(Guid id, string sessionId, string paymentIntentId);
    Task DeleteOrderHeaderAsync(OrderHeaderViewModel orderHeaderModel);
}