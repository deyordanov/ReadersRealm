namespace ReadersRealm.Data.Repositories.Contracts;

using Models;

public interface IOrderHeaderRepository : IRepository<OrderHeader>
{
    Task<OrderHeader?> GetByIdAsync(Guid id);
    Task<OrderHeader?> GetByIdWithNavPropertiesAsync(Guid id, string properties);
}