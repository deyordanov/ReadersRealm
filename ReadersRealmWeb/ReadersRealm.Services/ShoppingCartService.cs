namespace ReadersRealm.Services;

using Common.Exceptions;
using Contracts;
using Data.Models;
using Data.Repositories.Contracts;
using Microsoft.AspNetCore.Http;
using ViewModels.ApplicationUser;
using ViewModels.Book;
using ViewModels.OrderHeader;
using ViewModels.ShoppingCart;
using static Common.Constants.Constants.OrderHeader;

public class ShoppingCartService : IShoppingCartService
{
    private const string PropertiesToInclude = "Book, ApplicationUser";

    private readonly IUnitOfWork _unitOfWork;
    private readonly IApplicationUserService _applicationUserService;
    private readonly IOrderHeaderService _orderHeaderService;
    private readonly IOrderDetailsService _orderDetailsService;


    public ShoppingCartService(
        IUnitOfWork unitOfWork, 
        IApplicationUserService applicationUserService,
        IOrderHeaderService orderHeaderService,
        IOrderDetailsService orderDetailsService)
    {
        this._unitOfWork = unitOfWork;
        this._applicationUserService = applicationUserService;
        this._orderHeaderService = orderHeaderService;
        this._orderDetailsService = orderDetailsService;
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

    public async Task<int> GetShoppingCartCountByApplicationUserIdAsync(string applicationUserId)
    {
        IEnumerable<ShoppingCart> shoppingCarts = await this
            ._unitOfWork
            .ShoppingCartRepository
            .GetAsync(shoppingCart => shoppingCart.ApplicationUserId == applicationUserId,
                null, 
                "");

        return shoppingCarts.Count();
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
            ._unitOfWork
            .ShoppingCartRepository
            .AddAsync(shoppingCart);

        await this
            ._unitOfWork
            .SaveAsync();
    }

    public async Task UpdateShoppingCartCountAsync(ShoppingCartViewModel shoppingCartModel)
    {
        ShoppingCart? shoppingCart = await this
            ._unitOfWork
            .ShoppingCartRepository
            .GetByApplicationUserIdAndBookIdAsync(shoppingCartModel.ApplicationUserId, shoppingCartModel.BookId);

        if (shoppingCart == null)
        {
            throw new ShoppingCartNotFoundException();
        }

        shoppingCart.Count = shoppingCart.Count += shoppingCartModel.Count;

        await this
            ._unitOfWork
            .SaveAsync();
    }

    public async Task DeleteShoppingCartAsync(Guid id)
    {
        ShoppingCart? shoppingCartToDelete = await this
            ._unitOfWork
            .ShoppingCartRepository
            .GetByIdAsync(id);

        if (shoppingCartToDelete == null)
        {
            throw new ShoppingCartNotFoundException();
        }

        this
            ._unitOfWork
            .ShoppingCartRepository
            .Delete(shoppingCartToDelete);

        await this
            ._unitOfWork
            .SaveAsync();
    }

    public async Task DeleteAllShoppingCartsApplicationUserIdAsync(string applicationUserId)
    {
        List<ShoppingCart> shoppingCartsToDelete = await this
            ._unitOfWork
            .ShoppingCartRepository
            .GetAllByApplicationUserIdAsync(applicationUserId);

        this
            ._unitOfWork
            .ShoppingCartRepository
            .DeleteRange(shoppingCartsToDelete);

        await this
            ._unitOfWork
            .SaveAsync();
    }

    public async Task<bool> ShoppingCartExistsAsync(string applicationUserId, Guid bookId)
    {
        return await this
            ._unitOfWork
            .ShoppingCartRepository
            .GetFirstOrDefaultWithFilterAsync(shoppingCart => shoppingCart.ApplicationUserId == applicationUserId &&
                                                              shoppingCart.BookId == bookId) != null;
    }

    public async Task<AllShoppingCartsListViewModel> GetAllListAsync(string applicationUserId)
    {
        IEnumerable<ShoppingCart> allShoppingCarts = await this
            ._unitOfWork
            .ShoppingCartRepository
            .GetAsync(shoppingCart => shoppingCart.ApplicationUserId == applicationUserId, null, PropertiesToInclude);

        OrderApplicationUserViewModel applicationUser = await this._applicationUserService.GetApplicationUserForOrderAsync(applicationUserId);

        // OrderHeaderViewModel? orderHeaderModel = await this
        //     ._orderHeaderService
        //     .GetByApplicationUserIdAndOrderStatusAsync(applicationUser.Id, OrderStatusPending);
        //
        // if (orderHeaderModel != null)
        // {
        //     await this
        //         ._orderDetailsService
        //         .DeleteOrderDetailsRangeByOrderHeaderIdAsync(orderHeaderModel.Id);
        //
        //     await this
        //         ._orderHeaderService
        //         .DeleteOrderHeaderAsync(orderHeaderModel);
        // }

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
                    AuthorId = shoppingCart.Book.AuthorId,
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
            ._unitOfWork
            .ShoppingCartRepository
            .GetByIdAsync(shoppingCartId);

        if (shoppingCart == null)
        {
            throw new ShoppingCartNotFoundException();
        }

        shoppingCart.Count++;

        await this
            ._unitOfWork
            .SaveAsync();
    }

    public async Task<bool> DecreaseQuantityForShoppingCartAsync(Guid shoppingCartId)
    {
        ShoppingCart? shoppingCart = await this
            ._unitOfWork
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
                ._unitOfWork
                .SaveAsync();
        }
        else
        {
            await this.DeleteShoppingCartAsync(shoppingCartId);

            return true;
        }

        return false;
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