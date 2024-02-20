namespace ReadersRealm.Services.Data;

using Common;
using Common.Exceptions;
using ReadersRealm.Data.Models;
using ReadersRealm.Data.Repositories.Contracts;
using Contracts;
using ViewModels.ApplicationUser;
using ViewModels.Order;

public class OrderService : IOrderService
{
    private const string PropertiesToInclude = "OrderHeader, OrderDetailsList";

    private readonly IUnitOfWork _unitOfWork;
    private readonly IOrderHeaderService _orderHeaderService;
    private readonly IOrderDetailsService _orderDetailsService;
    private readonly IApplicationUserService _applicationUserService;

    public OrderService(
        IUnitOfWork unitOfWork,
        IOrderHeaderService orderHeaderService,
        IOrderDetailsService orderDetailsService,
        IApplicationUserService applicationUserService)
    {
        _unitOfWork = unitOfWork;
        _orderHeaderService = orderHeaderService;
        _orderDetailsService = orderDetailsService;
        _applicationUserService = applicationUserService;
    }

    public async Task<PaginatedList<AllOrdersViewModel>> GetAllAsync(int pageIndex, int pageSize, string? searchTerm)
    {
        List<Order> allOrders = await _unitOfWork
            .OrderRepository
            .GetAsync(null, null, PropertiesToInclude);

        List<AllOrdersViewModel> allOrderModelsList = new List<AllOrdersViewModel>();

        foreach (Order order in allOrders)
        {
            AllOrdersViewModel orderModel = new AllOrdersViewModel
            {
                Id = order.Id,
                OrderHeader = await _orderHeaderService.GetByIdAsyncWithNavPropertiesAsync(order.OrderHeaderId),
                OrderDetailsList = await _orderDetailsService.GetAllByOrderHeaderIdAsync(order.OrderHeaderId),
            };

            allOrderModelsList.Add(orderModel);
        }

        return PaginatedList<AllOrdersViewModel>.Create(allOrderModelsList, pageIndex, pageSize);
    }

    public async Task<PaginatedList<AllOrdersViewModel>> GetAllByUserIdAsync(int pageIndex, int pageSize, string? searchTerm, string userId)
    {
        List<Order> allOrders = await _unitOfWork
            .OrderRepository
            .GetAsync(order => order.OrderHeader.ApplicationUserId == userId, null, PropertiesToInclude);

        List<AllOrdersViewModel> allOrderModelsList = new List<AllOrdersViewModel>();

        foreach (Order order in allOrders)
        {
            AllOrdersViewModel orderModel = new AllOrdersViewModel
            {
                Id = order.Id,
                OrderHeader = await _orderHeaderService.GetByIdAsyncWithNavPropertiesAsync(order.OrderHeaderId),
                OrderDetailsList = await _orderDetailsService.GetAllByOrderHeaderIdAsync(order.OrderHeaderId)
            };

            allOrderModelsList.Add(orderModel);
        }

        return PaginatedList<AllOrdersViewModel>.Create(allOrderModelsList, pageIndex, pageSize);
    }

    public async Task<OrderViewModel> GetOrderForSummaryAsync(Guid id)
    {
        Order order = await GetByIdAsync(id);

        OrderViewModel orderModel = new OrderViewModel()
        {
            Id = order.Id,
            OrderHeaderId = order.OrderHeaderId,
        };
         
        return orderModel;
    }

    public async Task<DetailsOrderViewModel> GetOrderForDetailsAsync(Guid id)
    {
        Order order = await GetByIdAsync(id);

        DetailsOrderViewModel orderModel = new DetailsOrderViewModel
        {
            Id = order.Id,
            OrderHeaderId = order.OrderHeaderId,
            OrderHeader = await _orderHeaderService.GetByIdAsyncWithNavPropertiesAsync(order.OrderHeaderId),
            OrderDetailsList = await _orderDetailsService.GetAllByOrderHeaderIdAsync(order.OrderHeaderId)
        };

        return orderModel;
    }

    public async Task<Guid> GetOrderIdByOrderHeaderIdAsync(Guid orderHeaderId)
    {
        Order order = await GetByOrderHeaderIdAsync(orderHeaderId);

        return order.Id;
    }

    public async Task UpdateOrderAsync(DetailsOrderViewModel orderModel)
    {
        OrderApplicationUserViewModel applicationUserModel = new OrderApplicationUserViewModel()
        {
            Id = orderModel.OrderHeader.ApplicationUserId,
            FirstName = orderModel.OrderHeader.FirstName,
            LastName = orderModel.OrderHeader.LastName,
            PhoneNumber = orderModel.OrderHeader.PhoneNumber,
            City = orderModel.OrderHeader.City,
            PostalCode = orderModel.OrderHeader.PostalCode,
            State = orderModel.OrderHeader.State,
            StreetAddress = orderModel.OrderHeader.StreetAddress,
        };

        await _applicationUserService
            .UpdateApplicationUserAsync(applicationUserModel);

        await _orderHeaderService
            .UpdateOrderHeaderAsync(orderModel.OrderHeader);
    }

    private async Task<Order> GetByIdAsync(Guid id)
    {
        Order? order = await _unitOfWork
            .OrderRepository
            .GetByIdAsync(id);

        if (order == null)
        {
            throw new OrderNotFoundException();
        }

        return order;
    }

    private async Task<Order> GetByOrderHeaderIdAsync(Guid orderHeaderId)
    {
        Order? order = await _unitOfWork
            .OrderRepository
            .GetByOrderHeaderIdAsync(orderHeaderId);

        if (order == null)
        {
            throw new OrderNotFoundException();
        }

        return order;
    }
}