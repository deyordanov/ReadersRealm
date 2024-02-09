namespace ReadersRealm.Services.Contracts;

using ViewModels.Book;
using ViewModels.ShoppingCart;

public interface IShoppingCartService
{
    Task<ShoppingCartViewModel> GetShoppingCartByIdWithNavPropertiesOrCreateAsync(Guid id);
    ShoppingCartViewModel GetShoppingCart(DetailsBookViewModel bookModel);
    Task<bool> ShoppingCartExistsAsync(string applicationUserId, Guid bookId);
    Task<AllShoppingCartsListViewModel> GetAllListAsync(string applicationUserId);
    Task IncreaseQuantityForShoppingCartAsync(Guid shoppingCartId);
    Task DecreaseQuantityForShoppingCartAsync(Guid shoppingCartId);
    Task CreateShoppingCartAsync(ShoppingCartViewModel shoppingCartModel);
    Task UpdateShoppingCartCountAsync(ShoppingCartViewModel shoppingCartModel);
    Task DeleteShoppingCartAsync(Guid id);
    Task DeleteAllShoppingCartsApplicationUserIdAsync(string applicationUserId);
}