namespace ReadersRealm.Services;

using Common.Exceptions;
using Contracts;
using Data.Models;
using Data.Repositories.Contracts;
using ViewModels.ApplicationUser;
using ViewModels.Book;
using ViewModels.ShoppingCart;

public class ShoppingCartService : IShoppingCartService
{
    private IUnitOfWork unitOfWork;

    public ShoppingCartService(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
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

    public async Task<bool> ShoppingCartExistsAsync(string applicationUserId, Guid bookId)
    {
        return await this
            .unitOfWork
            .ShoppingCartRepository
            .GetFirstOrDefaultWithFilterAsync(shoppingCart => shoppingCart.ApplicationUserId == applicationUserId &&
                                                              shoppingCart.BookId == bookId) != null;
    }
}