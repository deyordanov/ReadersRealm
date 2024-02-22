﻿namespace ReadersRealm.Services.Data.OrderHeaderServices;

using Common.Exceptions.OrderHeader;
using Contracts;
using ReadersRealm.Data.Models;
using ReadersRealm.Data.Repositories.Contracts;
using Web.ViewModels.OrderHeader;

public class OrderHeaderCrudService : IOrderHeaderCrudService
{
    private readonly IUnitOfWork _unitOfWork;

    public OrderHeaderCrudService(IUnitOfWork unitOfWork)
    {
        this._unitOfWork = unitOfWork;
    }

    public async Task<(Guid orderHeaderId, Guid orderId)> CreateOrderHeaderAsync(OrderHeaderViewModel orderHeaderModel)
    {
        OrderHeader orderHeader = new OrderHeader()
        {
            ApplicationUserId = orderHeaderModel.ApplicationUserId,
            Carrier = orderHeaderModel.Carrier,
            OrderDate = orderHeaderModel.OrderDate,
            OrderStatus = orderHeaderModel.OrderStatus,
            OrderTotal = orderHeaderModel.OrderTotal,
            ShippingDate = orderHeaderModel.ShippingDate,
            SessionId = orderHeaderModel.SessionId,
            TrackingNumber = orderHeaderModel.TrackingNumber,
            PaymentDueDate = orderHeaderModel.PaymentDueDate,
            PaymentStatus = orderHeaderModel.PaymentStatus,
            PaymentIntentId = orderHeaderModel.PaymentIntentId,
            PaymentDate = orderHeaderModel.PaymentDate,
        };

        orderHeader.Order = new Order
        {
            OrderHeaderId = orderHeader.Id,
        };

        await this
            ._unitOfWork
            .OrderHeaderRepository
            .AddAsync(orderHeader);

        await this
            ._unitOfWork
            .SaveAsync();

        return (orderHeader.Id, orderHeader.Order.Id);
    }

    public async Task UpdateOrderHeaderAsync(OrderHeaderViewModel orderHeaderModel)
    {
        OrderHeader? orderHeader = await this
            ._unitOfWork
            .OrderHeaderRepository
            .GetByIdAsync(orderHeaderModel.Id);

        if (orderHeader == null)
        {
            throw new OrderHeaderNotFoundException();
        }

        orderHeader.ApplicationUserId = orderHeaderModel.ApplicationUserId;
        orderHeader.Carrier = orderHeaderModel.Carrier;
        orderHeader.OrderDate = orderHeaderModel.OrderDate;
        orderHeader.OrderStatus = orderHeaderModel.OrderStatus;
        orderHeader.OrderTotal = orderHeaderModel.OrderTotal;
        orderHeader.ShippingDate = orderHeaderModel.ShippingDate;
        orderHeader.SessionId = orderHeaderModel.SessionId;
        orderHeader.TrackingNumber = orderHeaderModel.TrackingNumber;
        orderHeader.PaymentDueDate = orderHeaderModel.PaymentDueDate;
        orderHeader.PaymentStatus = orderHeaderModel.PaymentStatus;
        orderHeader.PaymentIntentId = orderHeaderModel.PaymentIntentId;
        orderHeader.PaymentDate = orderHeaderModel.PaymentDate;

        await this
            ._unitOfWork
            .SaveAsync();
    }

    public async Task UpdateOrderHeaderStatusAsync(Guid id, string? orderStatus, string? paymentStatus)
    {
        OrderHeader? orderHeader = await this
            ._unitOfWork
            .OrderHeaderRepository
            .GetByIdAsync(id);

        if (orderHeader == null)
        {
            throw new OrderHeaderNotFoundException();
        }

        orderHeader.OrderStatus = orderStatus ?? orderHeader.OrderStatus;
        orderHeader.PaymentStatus = paymentStatus ?? orderHeader.PaymentStatus;

        await this
            ._unitOfWork
            .SaveAsync();
    }

    public async Task UpdateOrderHeaderPaymentIntentIdAsync(Guid id, string? sessionId, string? paymentIntentId)
    {
        OrderHeader? orderHeader = await this
            ._unitOfWork
            .OrderHeaderRepository
            .GetByIdAsync(id);

        if (orderHeader == null)
        {
            throw new OrderHeaderNotFoundException();
        }

        orderHeader.SessionId = sessionId ?? orderHeader.SessionId;
        orderHeader.PaymentIntentId = paymentIntentId ?? orderHeader.PaymentIntentId;

        await this
            ._unitOfWork
            .SaveAsync();
    }

    public async Task DeleteOrderHeaderAsync(OrderHeaderViewModel orderHeaderModel)
    {
        OrderHeader? orderHeaderToDelete = await this
            ._unitOfWork
            .OrderHeaderRepository
            .GetByIdAsync(orderHeaderModel.Id);

        if (orderHeaderToDelete == null)
        {
            throw new OrderHeaderNotFoundException();
        }

        this._unitOfWork
            .OrderHeaderRepository
            .Delete(orderHeaderToDelete);

        await this
            ._unitOfWork
            .SaveAsync();
    }
}