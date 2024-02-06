namespace ReadersRealm.Services.Contracts;

using ReadersRealm.ViewModels.Book;
using ViewModels.ShoppingCart;

public interface IShoppingCartService
{
    public Task<ShoppingCartViewModel> GetShoppingCartByIdWithNavPropertiesOrCreateAsync(Guid id);
    public ShoppingCartViewModel GetShoppingCart(DetailsBookViewModel bookModel);
    Task CreateShoppingCartAsync(ShoppingCartViewModel shoppingCartModel);

    Task UpdateShoppingCartCountAsync(ShoppingCartViewModel shoppingCartModel);

    Task<bool> ShoppingCartExistsAsync(string applicationUserId, Guid bookId);
}