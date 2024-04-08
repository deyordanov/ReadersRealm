namespace ReadersRealm.Services.Data.OrderDetailsServices;

using Contracts;
using Models.Book;
using Models.OrderDetails;
using ReadersRealm.Data.Models;
using ReadersRealm.Data.Repositories.Contracts;
using Web.ViewModels.Book;
using Web.ViewModels.OrderDetails;

public class OrderDetailsRetrievalService(IUnitOfWork unitOfWork) : IOrderDetailsRetrievalService
{
    private const string PropertiesToInclude = "Book, OrderHeader";

    public async Task<IEnumerable<OrderDetailsViewModel>> GetAllByOrderHeaderIdAsync(Guid orderHeaderId)
    {
        IEnumerable<OrderDetails> orderDetailsList = await unitOfWork
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
                    ImageId = orderDetails.Book.ImageId,
                    Pages = orderDetails.Book.Pages,
                    Price = orderDetails.Book.Price,
                    Used = orderDetails.Book.Used,
                },
                Count = orderDetails.Count,
                Price = orderDetails.Price,
            });

        return orderDetailsModelList;
    }

    public async Task<IEnumerable<OrderDetailsReceiptDto>> GetAllOrderDetailsForReceiptAsDtosAsync(Guid orderHeaderId)
    {
        IEnumerable<OrderDetails> orderDetailsList = await unitOfWork
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
                    ImageUrl = orderDetails.Book.ImageId,
                    Pages = orderDetails.Book.Pages,
                    Price = orderDetails.Book.Price,
                    Used = orderDetails.Book.Used,
                },
                Count = orderDetails.Count,
                Price = orderDetails.Price,
            });

        return orderDetailsDtoList;
    }
}