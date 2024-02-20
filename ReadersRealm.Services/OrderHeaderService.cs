namespace ReadersRealm.Services.Data;

using Common.Exceptions;
using ReadersRealm.Data.Models;
using ReadersRealm.Data.Repositories.Contracts;
using DataTransferObjects.OrderHeader;
using Contracts;
using ViewModels.ApplicationUser;
using ViewModels.OrderHeader;

public class OrderHeaderService : IOrderHeaderService
{
    private const string PropertiesToInclude = "ApplicationUser";

    private readonly IUnitOfWork _unitOfWork;
    private readonly IOrderDetailsService _orderDetailsService;

    public OrderHeaderService(
        IUnitOfWork unitOfWork, 
        IOrderDetailsService orderDetailsService)
    {
        _unitOfWork = unitOfWork;
        _orderDetailsService = orderDetailsService;
    }
    public async Task<OrderHeaderViewModel> GetByIdAsyncWithNavPropertiesAsync(Guid id)
    {
        OrderHeader? orderHeader = await _unitOfWork
            .OrderHeaderRepository
            .GetByIdWithNavPropertiesAsync(id, PropertiesToInclude);

        if (orderHeader == null)
        {
            throw new OrderHeaderNotFoundException();
        }

        OrderHeaderViewModel orderHeaderModel = new OrderHeaderViewModel()
        {
            Id = orderHeader.Id,
            ApplicationUserId = orderHeader.ApplicationUserId,
            ApplicationUser = new OrderApplicationUserViewModel()
            {
                Id = orderHeader.ApplicationUser.Id,
                FirstName = orderHeader.ApplicationUser.FirstName,
                LastName = orderHeader.ApplicationUser.LastName,
                City = orderHeader.ApplicationUser.City,
                PhoneNumber = orderHeader.ApplicationUser.PhoneNumber,
                PostalCode = orderHeader.ApplicationUser.PostalCode,
                State = orderHeader.ApplicationUser.State,
                StreetAddress = orderHeader.ApplicationUser.StreetAddress,
            },
            FirstName = orderHeader.ApplicationUser.FirstName,
            LastName = orderHeader.ApplicationUser.LastName,
            City = orderHeader.ApplicationUser.City ?? string.Empty,
            StreetAddress = orderHeader.ApplicationUser.StreetAddress ?? string.Empty,
            PostalCode = orderHeader.ApplicationUser.PostalCode ?? string.Empty,
            State = orderHeader.ApplicationUser.State ?? string.Empty,
            PhoneNumber = orderHeader.ApplicationUser.PhoneNumber ?? string.Empty,
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

    public async Task<OrderHeaderViewModel?> GetByApplicationUserIdAndOrderStatusAsync(string applicationUserId, string orderStatus)
    {
        IEnumerable<OrderHeader> orderHeaders = await _unitOfWork
            .OrderHeaderRepository
            .GetAsync(orderHeader => orderHeader.ApplicationUserId == applicationUserId &&
                                     orderHeader.OrderStatus == orderStatus, null, PropertiesToInclude);

        OrderHeader? orderHeader = orderHeaders.FirstOrDefault();

        if (orderHeader == null)
        {
            return null;
        }

        OrderHeaderViewModel orderHeaderModel = new OrderHeaderViewModel()
        {
            Id = orderHeader.Id,
            FirstName = orderHeader.ApplicationUser.FirstName,
            LastName = orderHeader.ApplicationUser.LastName,
            PhoneNumber = orderHeader.ApplicationUser.PhoneNumber ?? string.Empty,
            City = orderHeader.ApplicationUser.City ?? string.Empty,
            PostalCode = orderHeader.ApplicationUser.PostalCode ?? string.Empty,
            State = orderHeader.ApplicationUser.PostalCode ?? string.Empty,
            StreetAddress = orderHeader.ApplicationUser.StreetAddress ?? string.Empty,
            ApplicationUserId = orderHeader.ApplicationUserId,
            OrderStatus = orderHeader.OrderStatus,
            PaymentStatus = orderHeader.PaymentStatus,
            OrderTotal = orderHeader.OrderTotal,
            SessionId = orderHeader.SessionId,
            PaymentDueDate = orderHeader.PaymentDueDate,
            Carrier = orderHeader.Carrier,
            OrderDate = orderHeader.OrderDate,
            PaymentIntentId = orderHeader.PaymentIntentId,
            PaymentDate = orderHeader.PaymentDate,
            ShippingDate = orderHeader.ShippingDate,
            TrackingNumber = orderHeader.TrackingNumber,
        };

        return orderHeaderModel;
    }

    public async Task<OrderHeaderReceiptDto> GetOrderHeaderForReceiptAsync(Guid orderHeaderId)
    {
        OrderHeader? orderHeader = await _unitOfWork
            .OrderHeaderRepository
            .GetByIdAsync(orderHeaderId);

        if (orderHeader == null)
        {
            throw new OrderHeaderNotFoundException();
        }

        OrderHeaderReceiptDto orderHeaderModel = new OrderHeaderReceiptDto()
        {
            PaymentStatus = orderHeader.PaymentStatus!,
            OrderTotal = orderHeader.OrderTotal,
            PaymentIntentId = orderHeader.PaymentIntentId!,
            OrderDate = orderHeader.OrderDate,
            PaymentDate = orderHeader.PaymentDate,
            OrderDetails = await _orderDetailsService.GetAllOrderDetailsForReceiptAsDtosAsync(orderHeaderId),
        };

        return orderHeaderModel;
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

        await _unitOfWork
            .OrderHeaderRepository
            .AddAsync(orderHeader);

        await _unitOfWork
            .SaveAsync();

        return (orderHeader.Id, orderHeader.Order.Id);
    }

    public async Task UpdateOrderHeaderAsync(OrderHeaderViewModel orderHeaderModel)
    {
        OrderHeader? orderHeader = await _unitOfWork
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

        await _unitOfWork
            .SaveAsync();
    }

    public async Task UpdateOrderHeaderStatusAsync(Guid id, string? orderStatus, string? paymentStatus)
    {
        OrderHeader? orderHeader = await _unitOfWork
            .OrderHeaderRepository
            .GetByIdAsync(id);

        if (orderHeader == null)
        {
            throw new OrderHeaderNotFoundException();
        }

        orderHeader.OrderStatus = orderStatus ?? orderHeader.OrderStatus;
        orderHeader.PaymentStatus = paymentStatus ?? orderHeader.PaymentStatus;

        await _unitOfWork
            .SaveAsync();
    }

    public async Task UpdateOrderHeaderPaymentIntentIdAsync(Guid id, string? sessionId, string? paymentIntentId)
    {
        OrderHeader? orderHeader = await _unitOfWork
            .OrderHeaderRepository
            .GetByIdAsync(id);

        if (orderHeader == null)
        {
            throw new OrderHeaderNotFoundException();
        }

        orderHeader.SessionId = sessionId ?? orderHeader.SessionId;
        orderHeader.PaymentIntentId = paymentIntentId ?? orderHeader.PaymentIntentId;

        await _unitOfWork
            .SaveAsync();
    }

    public async Task DeleteOrderHeaderAsync(OrderHeaderViewModel orderHeaderModel)
    {
        OrderHeader? orderHeaderToDelete = await _unitOfWork
            .OrderHeaderRepository
            .GetByIdAsync(orderHeaderModel.Id);

        if (orderHeaderToDelete == null)
        {
            throw new OrderHeaderNotFoundException();
        }

        _unitOfWork
            .OrderHeaderRepository
            .Delete(orderHeaderToDelete);

        await _unitOfWork
            .SaveAsync();
    }
}