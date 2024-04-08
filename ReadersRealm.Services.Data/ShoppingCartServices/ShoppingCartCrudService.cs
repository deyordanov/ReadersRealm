namespace ReadersRealm.Services.Data.ShoppingCartServices;

using Common.Exceptions.ShoppingCart;
using Contracts;
using ReadersRealm.Data.Models;
using ReadersRealm.Data.Repositories.Contracts;
using Web.ViewModels.ShoppingCart;

public class ShoppingCartCrudService(IUnitOfWork unitOfWork) : IShoppingCartCrudService
{
    public async Task CreateShoppingCartAsync(ShoppingCartViewModel shoppingCartModel)
    {
        ShoppingCart shoppingCart = new ShoppingCart()
        {
            Id = shoppingCartModel.Id,
            ApplicationUserId = shoppingCartModel.ApplicationUserId,
            BookId = shoppingCartModel.BookId,
            Count = shoppingCartModel.Count,
        };

        await unitOfWork
            .ShoppingCartRepository
            .AddAsync(shoppingCart);

        await unitOfWork
            .SaveAsync();
    }

    public async Task UpdateShoppingCartCountAsync(ShoppingCartViewModel shoppingCartModel)
    {
        ShoppingCart? shoppingCart = await unitOfWork
            .ShoppingCartRepository
            .GetByApplicationUserIdAndBookIdAsync(shoppingCartModel.ApplicationUserId, shoppingCartModel.BookId);

        if (shoppingCart == null)
        {
            throw new ShoppingCartNotFoundException();
        }

        shoppingCart.Count = shoppingCart.Count += shoppingCartModel.Count;

        await unitOfWork
            .SaveAsync();
    }

    public async Task DeleteShoppingCartAsync(Guid id)
    {
        ShoppingCart? shoppingCartToDelete = await unitOfWork
            .ShoppingCartRepository
            .GetByIdAsync(id);

        if (shoppingCartToDelete == null)
        {
            throw new ShoppingCartNotFoundException();
        }

        unitOfWork
            .ShoppingCartRepository
            .Delete(shoppingCartToDelete);

        await unitOfWork
            .SaveAsync();
    }

    public async Task DeleteAllShoppingCartsApplicationUserIdAsync(Guid applicationUserId)
    {
        List<ShoppingCart> shoppingCartsToDelete = await unitOfWork
            .ShoppingCartRepository
            .GetAllByApplicationUserIdAsync(applicationUserId);

        unitOfWork
            .ShoppingCartRepository
            .DeleteRange(shoppingCartsToDelete);

        await unitOfWork
            .SaveAsync();
    }
}