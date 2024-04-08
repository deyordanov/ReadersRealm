namespace ReadersRealm.Services.Data.OrderDetailsServices;

using Contracts;
using ReadersRealm.Data.Models;
using ReadersRealm.Data.Repositories.Contracts;
using Web.ViewModels.OrderDetails;

public class OrderDetailsCrudService(
    IUnitOfWork unitOfWork,
    IOrderDetailsRetrievalService orderDetailsRetrievalService)
    : IOrderDetailsCrudService
{
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

        await unitOfWork
            .OrderDetailsRepository
            .AddAsync(orderDetails);

        await unitOfWork
            .SaveAsync();
    }

    public async Task DeleteOrderDetailsRangeByOrderHeaderIdAsync(Guid orderHeaderId)
    {
        IEnumerable<OrderDetailsViewModel> orderDetailsModelList = await orderDetailsRetrievalService
            .GetAllByOrderHeaderIdAsync(orderHeaderId);

        IEnumerable<OrderDetails> orderDetailsToDelete = orderDetailsModelList
            .Select(orderDetailsModel => new OrderDetails()
            {
                Id = orderDetailsModel.Id,
            });

        unitOfWork
            .OrderDetailsRepository
            .DeleteRange(orderDetailsToDelete);

        await unitOfWork
            .SaveAsync();
    }
}