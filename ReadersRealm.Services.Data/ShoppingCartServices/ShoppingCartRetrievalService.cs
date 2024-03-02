namespace ReadersRealm.Services.Data.ShoppingCartServices;

using ApplicationUserServices.Contracts;
using Contracts;
using ReadersRealm.Data.Models;
using ReadersRealm.Data.Repositories.Contracts;
using Web.ViewModels.ApplicationUser;
using Web.ViewModels.Book;
using Web.ViewModels.OrderHeader;
using Web.ViewModels.ShoppingCart;

public class ShoppingCartRetrievalService : IShoppingCartRetrievalService
{
    private const string PropertiesToInclude = "Book, ApplicationUser";

    private readonly IApplicationUserRetrievalService _applicationUserRetrievalService;
    private readonly IUnitOfWork _unitOfWork;

    public ShoppingCartRetrievalService(
        IUnitOfWork unitOfWork, 
        IApplicationUserRetrievalService applicationUserRetrievalService)
    {
        this._unitOfWork = unitOfWork;
        this._applicationUserRetrievalService = applicationUserRetrievalService;
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

    public async Task<int> GetShoppingCartCountByApplicationUserIdAsync(Guid applicationUserId)
    {
        IEnumerable<ShoppingCart> shoppingCarts = await this
            ._unitOfWork
            .ShoppingCartRepository
            .GetAsync(shoppingCart => shoppingCart.ApplicationUserId.Equals(applicationUserId),
                null,
                "");

        return shoppingCarts.Count();
    }

    public async Task<bool> ShoppingCartExistsAsync(Guid applicationUserId, Guid bookId)
    {
        return await this
            ._unitOfWork
            .ShoppingCartRepository
            .GetFirstOrDefaultWithFilterAsync(shoppingCart => shoppingCart.ApplicationUserId.Equals(applicationUserId) &&
                                                              shoppingCart.BookId == bookId, false) != null;
    }

    public async Task<AllShoppingCartsListViewModel> GetAllListAsync(Guid applicationUserId)
    {
        IEnumerable<ShoppingCart> allShoppingCarts = await this
            ._unitOfWork
            .ShoppingCartRepository
        .GetAsync(shoppingCart => shoppingCart.ApplicationUserId.Equals(applicationUserId), null, PropertiesToInclude);

        OrderApplicationUserViewModel applicationUser = await this
            ._applicationUserRetrievalService
            .GetApplicationUserForOrderAsync(applicationUserId);

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
                    ImageId = shoppingCart.Book.ImageId,
                    AuthorId = shoppingCart.Book.AuthorId,
                    CategoryId = shoppingCart.Book.CategoryId,
                },
                Count = shoppingCart.Count,
                TotalPrice = shoppingCart.Count * shoppingCart.Book.Price,
            }),
        };

        return shoppingCartModel;
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