﻿namespace ReadersRealm.Services.Data;

using ReadersRealm.Data.Models;
using ReadersRealm.Data.Repositories.Contracts;
using DataTransferObjects.Book;
using DataTransferObjects.OrderDetails;
using Contracts;
using ViewModels.Book;
using ViewModels.OrderDetails;

public class OrderDetailsService : IOrderDetailsService
{
    private const string PropertiesToInclude = "Book, OrderHeader";

    private readonly IUnitOfWork _unitOfWork;

    public OrderDetailsService(
        IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
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

        await _unitOfWork
            .OrderDetailsRepository
            .AddAsync(orderDetails);

        await _unitOfWork
            .SaveAsync();
    }

    public async Task<IEnumerable<OrderDetailsViewModel>> GetAllByOrderHeaderIdAsync(Guid orderHeaderId)
    {
        IEnumerable<OrderDetails> orderDetailsList = await _unitOfWork
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

    public async Task<IEnumerable<OrderDetailsReceiptDto>> GetAllOrderDetailsForReceiptAsDtosAsync(Guid orderHeaderId)
    {
        IEnumerable<OrderDetails> orderDetailsList = await _unitOfWork
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
        IEnumerable<OrderDetailsViewModel> orderDetailsModelList = await GetAllByOrderHeaderIdAsync(orderHeaderId);

        IEnumerable<OrderDetails> orderDetailsToDelete = orderDetailsModelList
            .Select(orderDetailsModel => new OrderDetails()
            {
                Id = orderDetailsModel.Id,
            });

        _unitOfWork
            .OrderDetailsRepository
            .DeleteRange(orderDetailsToDelete);

        await _unitOfWork
            .SaveAsync();
    }
}