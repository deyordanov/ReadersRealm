namespace ReadersRealm.Services.Data.ShoppingCartServices.Contracts;

using Web.ViewModels.Book;
using Web.ViewModels.ShoppingCart;

public interface IShoppingCartRetrievalService
{
    ShoppingCartViewModel GetShoppingCart(DetailsBookViewModel bookModel);
    Task<int> GetShoppingCartCountByApplicationUserIdAsync(string applicationUserId);
    Task<bool> ShoppingCartExistsAsync(string applicationUserId, Guid bookId);
    Task<AllShoppingCartsListViewModel> GetAllListAsync(string applicationUserId);
}