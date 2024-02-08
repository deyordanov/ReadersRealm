﻿namespace ReadersRealm.Services;

using Common.Exceptions;
using Contracts;
using Data.Models;
using Data.Repositories.Contracts;
using ViewModels.ApplicationUser;
using ViewModels.Book;
using ViewModels.OrderHeader;
using ViewModels.ShoppingCart;

public class ShoppingCartService : IShoppingCartService
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IApplicationUserService applicationUserService;

    public ShoppingCartService(IUnitOfWork unitOfWork, IApplicationUserService applicationUserService)
    {
        this.unitOfWork = unitOfWork;
        this.applicationUserService = applicationUserService;
    }

    public async Task<ShoppingCartViewModel> GetShoppingCartByIdWithNavPropertiesOrCreateAsync(Guid id)
    {
        ShoppingCart? shoppingCart = await this
            .unitOfWork
            .ShoppingCartRepository
            .GetByIdWithNavPropertiesAsync(id, "ApplicationUser, Book");

        if (shoppingCart == null)
        {
            shoppingCart = new ShoppingCart();
        }

        ShoppingCartViewModel viewModel = new ShoppingCartViewModel()
        {
            Id = shoppingCart!.Id,
            ApplicationUserId = shoppingCart.ApplicationUserId,
            BookId = shoppingCart.BookId,
        };

        return viewModel;
    }

    public ShoppingCartViewModel GetShoppingCart(DetailsBookViewModel bookModel)
    {
        return new ShoppingCartViewModel()
        {
            Count = 1,
            Book = bookModel,
            BookId = bookModel.Id,
        };
    }

    public async Task CreateShoppingCartAsync(ShoppingCartViewModel shoppingCartModel)
    {
        ShoppingCart shoppingCart = new ShoppingCart()
        {
            Id = shoppingCartModel.Id,
            ApplicationUserId =  shoppingCartModel.ApplicationUserId,
            BookId = shoppingCartModel.BookId,
            Count = shoppingCartModel.Count,
        };

        await this
            .unitOfWork
            .ShoppingCartRepository
            .AddAsync(shoppingCart);

        await this
            .unitOfWork
            .SaveAsync();
    }

    public async Task UpdateShoppingCartCountAsync(ShoppingCartViewModel shoppingCartModel)
    {
        ShoppingCart? shoppingCart = await this
            .unitOfWork
            .ShoppingCartRepository
            .GetByApplicationUserIdAndBookIdAsync(shoppingCartModel.ApplicationUserId, shoppingCartModel.BookId);

        if (shoppingCart == null)
        {
            throw new ShoppingCartNotFoundException();
        }

        shoppingCart.Count = shoppingCart.Count += shoppingCartModel.Count;

        await this
            .unitOfWork
            .SaveAsync();
    }

    public async Task DeleteShoppingCartAsync(Guid id)
    {
        ShoppingCart? shoppingCartToDelete = await this
            .unitOfWork
            .ShoppingCartRepository
            .GetByIdAsync(id);

        if (shoppingCartToDelete == null)
        {
            throw new ShoppingCartNotFoundException();
        }

        this
            .unitOfWork
            .ShoppingCartRepository
            .Delete(shoppingCartToDelete);

        await this
            .unitOfWork
            .SaveAsync();
    }

    public async Task<bool> ShoppingCartExistsAsync(string applicationUserId, Guid bookId)
    {
        return await this
            .unitOfWork
            .ShoppingCartRepository
            .GetFirstOrDefaultWithFilterAsync(shoppingCart => shoppingCart.ApplicationUserId == applicationUserId &&
                                                              shoppingCart.BookId == bookId) != null;
    }

    public async Task<AllShoppingCartsListViewModel> GetAllListAsync(string applicationUserId)
    {
        IEnumerable<ShoppingCart> allShoppingCarts = await this
            .unitOfWork
            .ShoppingCartRepository
            .GetAsync(shoppingCart => shoppingCart.ApplicationUserId == applicationUserId, null, "Book, ApplicationUser");

        OrderApplicationUserViewModel applicationUser = await this.applicationUserService.GetApplicationUserForOrderAsync(applicationUserId);

        AllShoppingCartsListViewModel shoppingCartModel = new AllShoppingCartsListViewModel()
        {
            OrderHeader = new OrderHeaderViewModel()
            {
                ApplicationUserId = applicationUserId,
                ApplicationUser = applicationUser,
                OrderTotal = allShoppingCarts.Sum(shoppingCart => this.CalculateShoppingCartTotal(shoppingCart.Count, shoppingCart.Book.Price)),
                FirstName = applicationUser.FirstName,
                LastName = applicationUser.LastName,
                City = applicationUser.City ?? string.Empty,
                StreetAddress = applicationUser.StreetAddress ?? string.Empty,
                PostalCode = applicationUser.PostalCode ?? string.Empty,
                State = applicationUser.State ?? string.Empty,
                PhoneNumber = applicationUser.PhoneNumber ?? string.Empty,
            },
            ShoppingCartsList = allShoppingCarts.Select(shoppingCart => new ShoppingCartViewModel()
            {
                Id = shoppingCart.Id,
                ApplicationUserId = applicationUserId,
                ApplicationUser = new ApplicationUserViewModel()
                {
                    Id = shoppingCart.ApplicationUser.Id,
                    FirstName = shoppingCart.ApplicationUser.FirstName,
                    LastName = shoppingCart.ApplicationUser.LastName,
                },
                BookId = shoppingCart.BookId,
                Book = new DetailsBookViewModel()
                {
                    Id = shoppingCart.Book.Id,
                    ISBN = shoppingCart.Book.ISBN,
                    Title = shoppingCart.Book.Title,
                    BookCover = shoppingCart.Book.BookCover,
                    Description = shoppingCart.Book.Description,
                    Pages = shoppingCart.Book.Pages,
                    Price = shoppingCart.Book.Price,
                    Used = shoppingCart.Book.Used,
                    ImageUrl = shoppingCart.Book.ImageUrl,
                    Author = shoppingCart.Book.Author,
                    AuthorId = shoppingCart.Book.AuthorId,
                    Category = shoppingCart.Book.Category,
                    CategoryId = shoppingCart.Book.CategoryId,
                },
                Count = shoppingCart.Count,
                TotalPrice = shoppingCart.Count * shoppingCart.Book.Price,
            }),
        };

        return shoppingCartModel;
    }

    public async Task IncreaseQuantityForShoppingCartAsync(Guid shoppingCartId)
    {
        ShoppingCart? shoppingCart = await this
            .unitOfWork
            .ShoppingCartRepository
            .GetByIdAsync(shoppingCartId);

        if (shoppingCart == null)
        {
            throw new ShoppingCartNotFoundException();
        }

        shoppingCart.Count++;

        await this
            .unitOfWork
            .SaveAsync();
    }

    public async Task DecreaseQuantityForShoppingCartAsync(Guid shoppingCartId)
    {
        ShoppingCart? shoppingCart = await this
            .unitOfWork
            .ShoppingCartRepository
            .GetByIdAsync(shoppingCartId);

        if (shoppingCart == null)
        {
            throw new ShoppingCartNotFoundException();
        }

        if (shoppingCart.Count > 1)
        {
            shoppingCart.Count--;

            await this
                .unitOfWork
                .SaveAsync();
        }
        else
        {
            await this.DeleteShoppingCartAsync(shoppingCartId);
        }
    }

    private decimal CalculateShoppingCartTotal(int count, decimal bookPrice)
    {
        decimal discount = count is >= 1 and <= 50
            ? 0M
            : count is >= 51 and <= 100
                ? 0.1M
                : 0.2M;

        decimal totalWithoutDiscount = bookPrice * count;
        return totalWithoutDiscount - (totalWithoutDiscount * discount);
    }
}