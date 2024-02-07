namespace ReadersRealm.Services.Contracts;

using ReadersRealm.ViewModels.Book;
using ReadersRealm.Web.ViewModels.Author;
using ViewModels.ShoppingCart;

public interface IShoppingCartService
{
    public Task<ShoppingCartViewModel> GetShoppingCartByIdWithNavPropertiesOrCreateAsync(Guid id);
    public ShoppingCartViewModel GetShoppingCart(DetailsBookViewModel bookModel);
    Task<bool> ShoppingCartExistsAsync(string applicationUserId, Guid bookId);
    Task<AllShoppingCartsListViewModel> GetAllListAsync(string applicationUserId);
    Task IncreaseQuantityForShoppingCartAsync(Guid shoppingCartId);
    Task DecreaseQuantityForShoppingCartAsync(Guid shoppingCartId);
    Task CreateShoppingCartAsync(ShoppingCartViewModel shoppingCartModel);
    Task UpdateShoppingCartCountAsync(ShoppingCartViewModel shoppingCartModel);
    Task DeleteShoppingCartAsync(Guid id);
}