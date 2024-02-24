namespace ReadersRealm.Services.Data.ShoppingCartServices.Contracts;

using Web.ViewModels.ShoppingCart;

public interface IShoppingCartCrudService
{
    Task CreateShoppingCartAsync(ShoppingCartViewModel shoppingCartModel);
    Task UpdateShoppingCartCountAsync(ShoppingCartViewModel shoppingCartModel);
    Task DeleteShoppingCartAsync(Guid id);
    Task DeleteAllShoppingCartsApplicationUserIdAsync(Guid applicationUserId);
}