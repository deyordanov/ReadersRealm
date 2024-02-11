namespace ReadersRealm.Services;

using Common;
using Common.Exceptions;
using Contracts;
using Data.Models;
using Data.Repositories.Contracts;
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
        this._unitOfWork = unitOfWork;
        this._orderHeaderService = orderHeaderService;
        this._orderDetailsService = orderDetailsService;
        this._applicationUserService = applicationUserService;
    }

    public async Task<PaginatedList<AllOrdersViewModel>> GetAllAsync(int pageIndex, int pageSize, string? searchTerm)
    {
        List<Order> allOrders = await this
            ._unitOfWork
            .OrderRepository
            .GetAsync(null, null, PropertiesToInclude);

        List<AllOrdersViewModel> allOrderModelsList = new List<AllOrdersViewModel>();

        foreach (Order order in allOrders)
        {
            AllOrdersViewModel orderModel = new AllOrdersViewModel
            {
                Id = order.Id,
                OrderHeader = await this._orderHeaderService.GetByIdAsyncWithNavPropertiesAsync(order.OrderHeaderId),
                OrderDetailsList = await this._orderDetailsService.GetAllByOrderHeaderIdAsync(order.OrderHeaderId),
            };

            allOrderModelsList.Add(orderModel);
        }

        return PaginatedList<AllOrdersViewModel>.Create(allOrderModelsList, pageIndex, pageSize);
    }

    public async Task<PaginatedList<AllOrdersViewModel>> GetAllByUserIdAsync(int pageIndex, int pageSize, string? searchTerm, string userId)
    {
        List<Order> allOrders = await this
            ._unitOfWork
            .OrderRepository
            .GetAsync(order => order.OrderHeader.ApplicationUserId == userId, null, PropertiesToInclude);

        List<AllOrdersViewModel> allOrderModelsList = new List<AllOrdersViewModel>();

        foreach (Order order in allOrders)
        {
            AllOrdersViewModel orderModel = new AllOrdersViewModel
            {
                Id = order.Id,
                OrderHeader = await this._orderHeaderService.GetByIdAsyncWithNavPropertiesAsync(order.OrderHeaderId),
                OrderDetailsList = await this._orderDetailsService.GetAllByOrderHeaderIdAsync(order.OrderHeaderId)
            };

            allOrderModelsList.Add(orderModel);
        }

        return PaginatedList<AllOrdersViewModel>.Create(allOrderModelsList, pageIndex, pageSize);
    }

    public async Task<OrderViewModel> GetOrderForSummaryAsync(Guid id)
    {
        Order order = await this
            .GetByIdAsync(id);

        OrderViewModel orderModel = new OrderViewModel()
        {
            Id = order.Id,
            OrderHeaderId = order.OrderHeaderId,
        };
         
        return orderModel;
    }

    public async Task<DetailsOrderViewModel> GetOrderForDetailsAsync(Guid id)
    {
        Order order = await this
            .GetByIdAsync(id);

        DetailsOrderViewModel orderModel = new DetailsOrderViewModel
        {
            Id = order.Id,
            OrderHeaderId = order.OrderHeaderId,
            OrderHeader = await this._orderHeaderService.GetByIdAsyncWithNavPropertiesAsync(order.OrderHeaderId),
            OrderDetailsList = await this._orderDetailsService.GetAllByOrderHeaderIdAsync(order.OrderHeaderId)
        };

        return orderModel;
    }

    public async Task<Guid> GetOrderIdByOrderHeaderIdAsync(Guid orderHeaderId)
    {
        Order order = await this
            .GetByOrderHeaderIdAsync(orderHeaderId);

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

        await this
            ._applicationUserService
            .UpdateApplicationUserAsync(applicationUserModel);

        await this
            ._orderHeaderService
            .UpdateOrderHeaderAsync(orderModel.OrderHeader);
    }

    private async Task<Order> GetByIdAsync(Guid id)
    {
        Order? order = await this
            ._unitOfWork
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
        Order? order = await this
            ._unitOfWork
            .OrderRepository
            .GetByOrderHeaderIdAsync(orderHeaderId);

        if (order == null)
        {
            throw new OrderNotFoundException();
        }

        return order;
    }
}