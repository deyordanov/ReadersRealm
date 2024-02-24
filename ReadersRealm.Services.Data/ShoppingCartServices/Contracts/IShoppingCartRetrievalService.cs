namespace ReadersRealm.Services.Data.ShoppingCartServices.Contracts;

using Web.ViewModels.Book;
using Web.ViewModels.ShoppingCart;

public interface IShoppingCartRetrievalService
{
    ShoppingCartViewModel GetShoppingCart(DetailsBookViewModel bookModel);
    Task<int> GetShoppingCartCountByApplicationUserIdAsync(Guid applicationUserId);
    Task<bool> ShoppingCartExistsAsync(Guid applicationUserId, Guid bookId);
    Task<AllShoppingCartsListViewModel> GetAllListAsync(Guid applicationUserId);
}