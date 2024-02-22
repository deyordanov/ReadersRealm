namespace ReadersRealm.Services.Data.OrderServices;

using Common;
using Common.Exceptions.Order;
using Contracts;
using OrderDetailsServices.Contracts;
using ReadersRealm.Data.Models;
using ReadersRealm.Data.Repositories.Contracts;
using ReadersRealm.Services.Data.OrderHeaderServices.Contracts;
using Web.ViewModels.Order;

public class OrderRetrievalService : IOrderRetrievalService
{
    private const string PropertiesToInclude = "OrderHeader, OrderDetailsList";

    private readonly IOrderDetailsRetrievalService _orderDetailsRetrievalService;
    private readonly IOrderHeaderRetrievalService _orderHeaderRetrievalService;
    private readonly IUnitOfWork _unitOfWork;

    public OrderRetrievalService(
        IOrderDetailsRetrievalService orderDetailsRetrievalService,
        IOrderHeaderRetrievalService orderHeaderRetrievalService, 
        IUnitOfWork unitOfWork)
    {
        this._orderDetailsRetrievalService = orderDetailsRetrievalService;
        this._orderHeaderRetrievalService = orderHeaderRetrievalService;
        this._unitOfWork = unitOfWork;
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
                OrderHeader = await this
                    ._orderHeaderRetrievalService
                    .GetByIdAsyncWithNavPropertiesAsync(order.OrderHeaderId),
                OrderDetailsList = await this
                    ._orderDetailsRetrievalService
                    .GetAllByOrderHeaderIdAsync(order.OrderHeaderId),
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
                OrderHeader = await this
                    ._orderHeaderRetrievalService
                    .GetByIdAsyncWithNavPropertiesAsync(order.OrderHeaderId),
                OrderDetailsList = await this
                    ._orderDetailsRetrievalService
                    .GetAllByOrderHeaderIdAsync(order.OrderHeaderId)
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
            OrderHeader = await this
                ._orderHeaderRetrievalService
                .GetByIdAsyncWithNavPropertiesAsync(order.OrderHeaderId),
            OrderDetailsList = await this
                ._orderDetailsRetrievalService
                .GetAllByOrderHeaderIdAsync(order.OrderHeaderId)
        };

        return orderModel;
    }

    public async Task<Guid> GetOrderIdByOrderHeaderIdAsync(Guid orderHeaderId)
    {
        Order order = await GetByOrderHeaderIdAsync(orderHeaderId);

        return order.Id;
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