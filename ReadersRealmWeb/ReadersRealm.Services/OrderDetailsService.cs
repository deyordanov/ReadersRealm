namespace ReadersRealm.Services;

using Contracts;
using Data.Models;
using Data.Repositories.Contracts;
using Web.ViewModels.OrderDetails;

public class OrderDetailsService : IOrderDetailsService
{
    private const string PropertiesToInclude = "Book, OrderHeader";

    private readonly IUnitOfWork _unitOfWork;

    public OrderDetailsService(IUnitOfWork unitOfWork)
    {
        this._unitOfWork = unitOfWork;
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
            ._unitOfWork
            .OrderDetailsRepository
            .AddAsync(orderDetails);

        await this
            ._unitOfWork
            .SaveAsync();
    }

    public async Task<IEnumerable<OrderDetailsViewModel>> GetAllByOrderHeaderAsync(Guid orderHeaderId)
    {
        IEnumerable<OrderDetails> allOrderDetails = await this
            ._unitOfWork
            .OrderDetailsRepository
            .GetAsync(orderDetails => orderDetails.OrderHeaderId == orderHeaderId, null, PropertiesToInclude);

        return allOrderDetails.Select(orderDetails => new OrderDetailsViewModel()
        {
            Id = orderDetails.Id,
            OrderHeaderId = orderDetails.OrderHeaderId,
            OrderHeader = orderDetails.OrderHeader,
            Book = orderDetails.Book,
            BookId = orderDetails.BookId,
            Price = orderDetails.Price,
            Count = orderDetails.Count,
        });
    }
}