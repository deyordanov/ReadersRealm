namespace ReadersRealm.Services.Data.ShoppingCartServices;

using Common.Exceptions.ShoppingCart;
using Contracts;
using ReadersRealm.Data.Models;
using ReadersRealm.Data.Repositories.Contracts;

public class ShoppingCartModificationService(
    IUnitOfWork unitOfWork,
    IShoppingCartCrudService shoppingCartCrudService)
    : IShoppingCartModificationService
{
    public async Task IncreaseShoppingCartQuantityAsync(Guid shoppingCartId)
    {
        ShoppingCart? shoppingCart = await unitOfWork
            .ShoppingCartRepository
            .GetByIdAsync(shoppingCartId);

        if (shoppingCart == null)
        {
            throw new ShoppingCartNotFoundException();
        }

        shoppingCart.Count++;

        await unitOfWork
            .SaveAsync();
    }

    public async Task<bool> DecreaseShoppingCartQuantityAsync(Guid shoppingCartId)
    {
        ShoppingCart? shoppingCart = await unitOfWork
            .ShoppingCartRepository
            .GetByIdAsync(shoppingCartId);

        if (shoppingCart == null)
        {
            throw new ShoppingCartNotFoundException();
        }

        if (shoppingCart.Count > 1)
        {
            shoppingCart.Count--;

            await unitOfWork
                .SaveAsync();
        }
        else
        {
            await shoppingCartCrudService
                .DeleteShoppingCartAsync(shoppingCartId);

            return true;
        }

        return false;
    }
}