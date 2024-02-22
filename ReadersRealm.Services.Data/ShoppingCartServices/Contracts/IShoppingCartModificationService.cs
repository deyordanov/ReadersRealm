namespace ReadersRealm.Services.Data.ShoppingCartServices.Contracts;

public interface IShoppingCartModificationService
{
    Task IncreaseShoppingCartQuantityAsync(Guid shoppingCartId);
    Task<bool> DecreaseShoppingCartQuantityAsync(Guid shoppingCartId);
}