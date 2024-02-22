namespace ReadersRealm.Services.Data.OrderDetailsServices;

using Contracts;
using ReadersRealm.Data.Models;
using ReadersRealm.Data.Repositories.Contracts;
using Web.ViewModels.OrderDetails;

public class OrderDetailsCrudService : IOrderDetailsCrudService
{
    private readonly IOrderDetailsRetrievalService _orderDetailsRetrievalService;
    private readonly IUnitOfWork _unitOfWork;

    public OrderDetailsCrudService(
        IUnitOfWork unitOfWork, 
        IOrderDetailsRetrievalService orderDetailsRetrievalService)
    {
        this._unitOfWork = unitOfWork;
        this._orderDetailsRetrievalService = orderDetailsRetrievalService;
    }

    public async Task CreateOrderDetailsAsync(OrderDetailsViewModel orderDetailsModel)
    {
        OrderDetails orderDetails = new OrderDetails()
        {
            BookId = orderDetailsModel.BookId,
            OrderHeaderId = orderDetailsModel.OrderHeaderId,
            Count = orderDetailsModel.Count,
            Price = orderDetailsModel.Price,
            OrderId = orderDetailsModel.Order!.Id,
        };

        await this
            ._unitOfWork
            .OrderDetailsRepository
            .AddAsync(orderDetails);

        await this
            ._unitOfWork
            .SaveAsync();
    }

    public async Task DeleteOrderDetailsRangeByOrderHeaderIdAsync(Guid orderHeaderId)
    {
        IEnumerable<OrderDetailsViewModel> orderDetailsModelList = await this
            ._orderDetailsRetrievalService
            .GetAllByOrderHeaderIdAsync(orderHeaderId);

        IEnumerable<OrderDetails> orderDetailsToDelete = orderDetailsModelList
            .Select(orderDetailsModel => new OrderDetails()
            {
                Id = orderDetailsModel.Id,
            });

        this._unitOfWork
            .OrderDetailsRepository
            .DeleteRange(orderDetailsToDelete);

        await this
            ._unitOfWork
            .SaveAsync();
    }
}