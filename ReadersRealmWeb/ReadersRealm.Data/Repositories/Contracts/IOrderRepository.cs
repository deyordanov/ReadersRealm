namespace ReadersRealm.Data.Repositories.Contracts;

using Models;

public interface IOrderRepository : IRepository<Order>
{
    Task<Order?> GetByIdAsync(Guid id);
    Task<Order?> GetByIdWithNavPropertiesAsync(Guid id, string properties);
}