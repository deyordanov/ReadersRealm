namespace ReadersRealm.Data.Repositories.Contracts;

using Models;
using System.Linq.Expressions;

public interface IShoppingCartRepository : IRepository<ShoppingCart>
{
    Task<ShoppingCart?> GetByIdAsync(Guid id);
    Task<ShoppingCart?> GetByIdWithNavPropertiesAsync(Guid id, string properties);
    Task<ShoppingCart?> GetFirstOrDefaultWithFilterAsync(Expression<Func<ShoppingCart, bool>> filter, bool tracking);
    Task<ShoppingCart?> GetByApplicationUserIdAndBookIdAsync(Guid applicationUserId, Guid bookId);
    Task<List<ShoppingCart>> GetAllByApplicationUserIdAsync(Guid applicationUserId);
}