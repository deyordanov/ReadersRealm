namespace ReadersRealm.Services.Data.ShoppingCartServices;

using Common.Exceptions.ShoppingCart;
using Contracts;
using ReadersRealm.Data.Models;
using ReadersRealm.Data.Repositories.Contracts;
using Web.ViewModels.ShoppingCart;

public class ShoppingCartCrudService : IShoppingCartCrudService
{
    private readonly IUnitOfWork _unitOfWork;

    public ShoppingCartCrudService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task CreateShoppingCartAsync(ShoppingCartViewModel shoppingCartModel)
    {
        ShoppingCart shoppingCart = new ShoppingCart()
        {
            Id = shoppingCartModel.Id,
            ApplicationUserId = shoppingCartModel.ApplicationUserId,
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

    public async Task DeleteAllShoppingCartsApplicationUserIdAsync(Guid applicationUserId)
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
}