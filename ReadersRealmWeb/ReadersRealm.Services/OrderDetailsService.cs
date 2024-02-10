namespace ReadersRealm.Services;

using Contracts;
using Data.Models;
using Data.Repositories.Contracts;
using DataTransferObjects.Book;
using DataTransferObjects.OrderHeader;
using ReadersRealm.DataTransferObjects.OrderDetails;
using ViewModels.Book;
using ViewModels.OrderDetails;

public class OrderDetailsService : IOrderDetailsService
{
    private const string PropertiesToInclude = "Book, OrderHeader";

    private readonly IUnitOfWork _unitOfWork;

    public OrderDetailsService(
        IUnitOfWork unitOfWork)
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
            Order = new Order()
            {
                OrderHeaderId = orderDetailsModel.OrderHeaderId,
            },
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
        IEnumerable<OrderDetails> orderDetailsList = await this
            ._unitOfWork
            .OrderDetailsRepository
            .GetAsync(orderDetails => orderDetails.OrderHeaderId == orderHeaderId,
                null,
                PropertiesToInclude);

        IEnumerable<OrderDetailsViewModel> orderDetailsModelList = orderDetailsList
            .Select(orderDetails => new OrderDetailsViewModel()
            {
                Id = orderDetails.Id,
                OrderHeaderId = orderDetails.OrderHeaderId,
                BookId = orderDetails.BookId,
                Book = new BookViewModel()
                {
                    ISBN = orderDetails.Book.ISBN,
                    Title = orderDetails.Book.Title,
                    AuthorId = orderDetails.Book.AuthorId,
                    BookCover = orderDetails.Book.BookCover,
                    CategoryId = orderDetails.Book.CategoryId,
                    Description = orderDetails.Book.Description,
                    Id = orderDetails.Book.Id,
                    ImageUrl = orderDetails.Book.ImageUrl,
                    Pages = orderDetails.Book.Pages,
                    Price = orderDetails.Book.Price,
                    Used = orderDetails.Book.Used,
                },
                Count = orderDetails.Count,
                Price = orderDetails.Price,
            });

        return orderDetailsModelList;
    }

    public async Task<IEnumerable<OrderDetailsDto>> GetAllByOrderHeaderAsDtosAsync(Guid orderHeaderId)
    {
        IEnumerable<OrderDetails> orderDetailsLit = await this
            ._unitOfWork
            .OrderDetailsRepository
            .GetAsync(orderDetails => orderDetails.OrderHeaderId == orderHeaderId,
                null,
                PropertiesToInclude);

        throw new NotImplementedException();
    }

    public async Task<IEnumerable<OrderDetailsReceiptDto>> GetAllOrderDetailsForReceiptAsDtosAsync(Guid orderHeaderId)
    {
        IEnumerable<OrderDetails> orderDetailsList = await this
            ._unitOfWork
            .OrderDetailsRepository
            .GetAsync(orderDetails => orderDetails.OrderHeaderId == orderHeaderId,
                null,
                PropertiesToInclude);

        IEnumerable<OrderDetailsReceiptDto> orderDetailsDtoList = orderDetailsList
            .Select(orderDetails => new OrderDetailsReceiptDto()
            {
                Id = orderDetails.Id,
                OrderHeaderId = orderDetails.OrderHeaderId,
                BookId = orderDetails.BookId,
                Book = new BookDto()
                {
                    ISBN = orderDetails.Book.ISBN,
                    Title = orderDetails.Book.Title,
                    AuthorId = orderDetails.Book.AuthorId,
                    BookCover = orderDetails.Book.BookCover,
                    CategoryId = orderDetails.Book.CategoryId,
                    Description = orderDetails.Book.Description,
                    Id = orderDetails.Book.Id,
                    ImageUrl = orderDetails.Book.ImageUrl,
                    Pages = orderDetails.Book.Pages,
                    Price = orderDetails.Book.Price,
                    Used = orderDetails.Book.Used,
                },
                Count = orderDetails.Count,
                Price = orderDetails.Price,
            });

        return orderDetailsDtoList;
    }

    public async Task DeleteOrderDetailsRangeByOrderHeaderIdAsync(Guid orderHeaderId)
    {
        IEnumerable<OrderDetailsViewModel> orderDetailsModelList = await this
            .GetAllByOrderHeaderAsync(orderHeaderId);

        IEnumerable<OrderDetails> orderDetailsToDelete = orderDetailsModelList
            .Select(orderDetailsModel => new OrderDetails()
            {
                Id = orderDetailsModel.Id,
            });

        this
            ._unitOfWork
            .OrderDetailsRepository
            .DeleteRange(orderDetailsToDelete);

        await this
            ._unitOfWork
            .SaveAsync();
    }
}