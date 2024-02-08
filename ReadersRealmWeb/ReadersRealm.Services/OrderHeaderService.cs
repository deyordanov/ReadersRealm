namespace ReadersRealm.Services;

using Common.Exceptions;
using Contracts;
using Data.Models;
using Data.Repositories.Contracts;
using ViewModels.ApplicationUser;
using ViewModels.OrderHeader;

public class OrderHeaderService : IOrderHeaderService
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IApplicationUserService applicationUserService;

    public OrderHeaderService(IUnitOfWork unitOfWork, IApplicationUserService applicationUserService)
    {
        this.unitOfWork = unitOfWork;
        this.applicationUserService = applicationUserService;
    }
    public async Task<OrderHeaderViewModel> GetByIdAsyncWithNavPropertiesAsync(Guid id)
    {
        OrderHeader? orderHeader = await this
            .unitOfWork
            .OrderHeaderRepository
            .GetByIdAsync(id);

        if (orderHeader == null)
        {
            throw new OrderHeaderNotFoundException();
        }

        OrderApplicationUserViewModel applicationUser = await this
            .applicationUserService
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
            TrackingNumber = orderHeaderModel.TrackingNumber,
            PaymentDueDate = orderHeaderModel.PaymentDueDate,
            PaymentStatus = orderHeaderModel.PaymentStatus,
            PaymentIntentId = orderHeaderModel.PaymentIntentId,
            PaymentDate = orderHeaderModel.PaymentDate,
        };

        await this
            .unitOfWork
            .OrderHeaderRepository
            .AddAsync(orderHeader);

        await this
            .unitOfWork
            .SaveAsync();

        return orderHeader.Id;
    }
}