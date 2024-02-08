namespace ReadersRealm.Services;

using Common.Exceptions;
using Contracts;
using Data.Models;
using Data.Repositories.Contracts;
using ViewModels.ApplicationUser;
using ViewModels.OrderHeader;

public class OrderHeaderService : IOrderHeaderService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IApplicationUserService _applicationUserService;

    public OrderHeaderService(IUnitOfWork unitOfWork, IApplicationUserService applicationUserService)
    {
        this._unitOfWork = unitOfWork;
        this._applicationUserService = applicationUserService;
    }
    public async Task<OrderHeaderViewModel> GetByIdAsyncWithNavPropertiesAsync(Guid id)
    {
        OrderHeader? orderHeader = await this
            ._unitOfWork
            .OrderHeaderRepository
            .GetByIdAsync(id);

        if (orderHeader == null)
        {
            throw new OrderHeaderNotFoundException();
        }

        OrderApplicationUserViewModel applicationUser = await this
            ._applicationUserService
            .GetApplicationUserForOrderAsync(orderHeader.ApplicationUserId);

        OrderHeaderViewModel orderHeaderModel = new OrderHeaderViewModel()
        {
            Id = orderHeader.Id,
            ApplicationUserId = orderHeader.ApplicationUserId,
            ApplicationUser = applicationUser,
            FirstName = applicationUser.FirstName,
            LastName = applicationUser.LastName,
            City = applicationUser.City ?? string.Empty,
            StreetAddress = applicationUser.StreetAddress ?? string.Empty,
            PostalCode = applicationUser.PostalCode ?? string.Empty,
            State = applicationUser.State ?? string.Empty,
            PhoneNumber = applicationUser.PhoneNumber ?? string.Empty,
            OrderTotal = orderHeader.OrderTotal,
            OrderStatus = orderHeader.OrderStatus,
            OrderDate = orderHeader.OrderDate,
            Carrier = orderHeader.Carrier,
            PaymentDate = orderHeader.PaymentDate,
            PaymentDueDate = orderHeader.PaymentDueDate,
            PaymentIntentId = orderHeader.PaymentIntentId,
            PaymentStatus = orderHeader.PaymentStatus,
            SessionId = orderHeader.SessionId,
            ShippingDate = orderHeader.ShippingDate,
            TrackingNumber = orderHeader.TrackingNumber,
        };

        return orderHeaderModel;
    }

    public async Task<Guid> CreateOrderHeaderAsync(OrderHeaderViewModel orderHeaderModel)
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

        await this
            ._unitOfWork
            .OrderHeaderRepository
            .AddAsync(orderHeader);

        await this
            ._unitOfWork
            .SaveAsync();

        return orderHeader.Id;
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
}