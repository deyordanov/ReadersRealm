namespace ReadersRealm.Services.Data;

using Common.Exceptions.ShoppingCart;
using Contracts;
using ReadersRealm.Data.Models;
using ReadersRealm.Data.Repositories.Contracts;
using ViewModels.ApplicationUser;
using ViewModels.Book;
using ViewModels.OrderHeader;
using ViewModels.ShoppingCart;

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
        _unitOfWork = unitOfWork;
        _applicationUserService = applicationUserService;
        _orderHeaderService = orderHeaderService;
        _orderDetailsService = orderDetailsService;
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
        IEnumerable<ShoppingCart> shoppingCarts = await _unitOfWork
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

        await _unitOfWork
            .ShoppingCartRepository
            .AddAsync(shoppingCart);

        await _unitOfWork
            .SaveAsync();
    }

    public async Task UpdateShoppingCartCountAsync(ShoppingCartViewModel shoppingCartModel)
    {
        ShoppingCart? shoppingCart = await _unitOfWork
            .ShoppingCartRepository
            .GetByApplicationUserIdAndBookIdAsync(shoppingCartModel.ApplicationUserId, shoppingCartModel.BookId);

        if (shoppingCart == null)
        {
            throw new ShoppingCartNotFoundException();
        }

        shoppingCart.Count = shoppingCart.Count += shoppingCartModel.Count;

        await _unitOfWork
            .SaveAsync();
    }

    public async Task DeleteShoppingCartAsync(Guid id)
    {
        ShoppingCart? shoppingCartToDelete = await _unitOfWork
            .ShoppingCartRepository
            .GetByIdAsync(id);

        if (shoppingCartToDelete == null)
        {
            throw new ShoppingCartNotFoundException();
        }

        _unitOfWork
            .ShoppingCartRepository
            .Delete(shoppingCartToDelete);

        await _unitOfWork
            .SaveAsync();
    }

    public async Task DeleteAllShoppingCartsApplicationUserIdAsync(string applicationUserId)
    {
        List<ShoppingCart> shoppingCartsToDelete = await _unitOfWork
            .ShoppingCartRepository
            .GetAllByApplicationUserIdAsync(applicationUserId);

        _unitOfWork
            .ShoppingCartRepository
            .DeleteRange(shoppingCartsToDelete);

        await _unitOfWork
            .SaveAsync();
    }

    public async Task<bool> ShoppingCartExistsAsync(string applicationUserId, Guid bookId)
    {
        return await _unitOfWork
            .ShoppingCartRepository
            .GetFirstOrDefaultWithFilterAsync(shoppingCart => shoppingCart.ApplicationUserId == applicationUserId &&
                                                              shoppingCart.BookId == bookId) != null;
    }

    public async Task<AllShoppingCartsListViewModel> GetAllListAsync(string applicationUserId)
    {
        IEnumerable<ShoppingCart> allShoppingCarts = await _unitOfWork
            .ShoppingCartRepository
            .GetAsync(shoppingCart => shoppingCart.ApplicationUserId == applicationUserId, null, PropertiesToInclude);

        OrderApplicationUserViewModel applicationUser = await _applicationUserService.GetApplicationUserForOrderAsync(applicationUserId);

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
                OrderTotal = allShoppingCarts.Sum(shoppingCart => CalculateShoppingCartTotal(shoppingCart.Count, shoppingCart.Book.Price)),
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
        ShoppingCart? shoppingCart = await _unitOfWork
            .ShoppingCartRepository
            .GetByIdAsync(shoppingCartId);

        if (shoppingCart == null)
        {
            throw new ShoppingCartNotFoundException();
        }

        shoppingCart.Count++;

        await _unitOfWork
            .SaveAsync();
    }

    public async Task<bool> DecreaseQuantityForShoppingCartAsync(Guid shoppingCartId)
    {
        ShoppingCart? shoppingCart = await _unitOfWork
            .ShoppingCartRepository
            .GetByIdAsync(shoppingCartId);

        if (shoppingCart == null)
        {
            throw new ShoppingCartNotFoundException();
        }

        if (shoppingCart.Count > 1)
        {
            shoppingCart.Count--;

            await _unitOfWork
                .SaveAsync();
        }
        else
        {
            await DeleteShoppingCartAsync(shoppingCartId);

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