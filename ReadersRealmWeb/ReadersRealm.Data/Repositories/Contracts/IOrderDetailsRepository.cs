namespace ReadersRealm.Data.Repositories.Contracts;

using Models;

public interface IOrderDetailsRepository : IRepository<OrderDetails>
{
    Task<OrderDetails?> GetByIdAsync(Guid id);
}