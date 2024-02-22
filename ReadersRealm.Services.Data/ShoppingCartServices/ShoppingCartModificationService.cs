namespace ReadersRealm.Services.Data.ShoppingCartServices;

using Common.Exceptions.ShoppingCart;
using Contracts;
using ReadersRealm.Data.Models;
using ReadersRealm.Data.Repositories.Contracts;

public class ShoppingCartModificationService : IShoppingCartModificationService
{
    private readonly IShoppingCartCrudService _shoppingCartCrudService;
    private readonly IUnitOfWork _unitOfWork;

    public ShoppingCartModificationService(
        IUnitOfWork unitOfWork, 
        IShoppingCartCrudService shoppingCartCrudService)
    {
        this._unitOfWork = unitOfWork;
        _shoppingCartCrudService = shoppingCartCrudService;
    }

    public async Task IncreaseShoppingCartQuantityAsync(Guid shoppingCartId)
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

    public async Task<bool> DecreaseShoppingCartQuantityAsync(Guid shoppingCartId)
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
            await this
                ._shoppingCartCrudService
                .DeleteShoppingCartAsync(shoppingCartId);

            return true;
        }

        return false;
    }
}