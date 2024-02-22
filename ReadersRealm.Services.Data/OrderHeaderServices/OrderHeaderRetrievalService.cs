namespace ReadersRealm.Services.Data.OrderHeaderServices;

using Contracts;
using Models.OrderHeader;
using OrderDetailsServices.Contracts;
using Common.Exceptions.OrderHeader;
using ReadersRealm.Data.Models;
using ReadersRealm.Data.Repositories.Contracts;
using Web.ViewModels.ApplicationUser;
using Web.ViewModels.OrderHeader;

public class OrderHeaderRetrievalService : IOrderHeaderRetrievalService
{
    private const string PropertiesToInclude = "ApplicationUser";

    private readonly IOrderDetailsRetrievalService _orderDetailsRetrievalService;
    private readonly IUnitOfWork _unitOfWork;

    public OrderHeaderRetrievalService(
        IUnitOfWork unitOfWork, 
        IOrderDetailsRetrievalService orderDetailsRetrievalService)
    {
        this._unitOfWork = unitOfWork;
        this._orderDetailsRetrievalService = orderDetailsRetrievalService;
    }

    public async Task<OrderHeaderViewModel> GetByIdAsyncWithNavPropertiesAsync(Guid id)
    {
        OrderHeader? orderHeader = await this
            ._unitOfWork
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
                Email = orderHeader.ApplicationUser.UserName!,
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
        IEnumerable<OrderHeader> orderHeaders = await this
            ._unitOfWork
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
        OrderHeader? orderHeader = await this
            ._unitOfWork
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
            OrderDetails = await this
                ._orderDetailsRetrievalService
                .GetAllOrderDetailsForReceiptAsDtosAsync(orderHeaderId),
        };

        return orderHeaderModel;
    }
}