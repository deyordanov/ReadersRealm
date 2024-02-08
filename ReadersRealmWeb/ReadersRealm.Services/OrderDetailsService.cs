namespace ReadersRealm.Services;

using Contracts;
using Data.Models;
using Data.Repositories.Contracts;
using Web.ViewModels.OrderDetails;

public class OrderDetailsService : IOrderDetailsService
{
    private readonly IUnitOfWork unitOfWork;

    public OrderDetailsService(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }


    public async Task CreateOrderDetailsAsync(OrderDetailsViewModel orderDetailsModel)
    {
        OrderDetails orderDetails = new OrderDetails()
        {
            BookId = orderDetailsModel.BookId,
            OrderHeaderId = orderDetailsModel.OrderHeaderId,
            Count = orderDetailsModel.Count,
            Price = orderDetailsModel.Price,
        };

        await this
            .unitOfWork
            .OrderDetailsRepository
            .AddAsync(orderDetails);

        await this
            .unitOfWork
            .SaveAsync();
    }
}