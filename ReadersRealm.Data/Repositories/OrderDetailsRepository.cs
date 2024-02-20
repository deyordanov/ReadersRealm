namespace ReadersRealm.Data.Repositories;

using Contracts;
using Microsoft.EntityFrameworkCore;
using Models;

public class OrderDetailsRepository : Repository<OrderDetails>, IOrderDetailsRepository
{
    private readonly ReadersRealmDbContext _dbContext;
    public OrderDetailsRepository(ReadersRealmDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<OrderDetails?> GetByIdAsync(Guid id)
    {
        return await _dbContext
            .OrdersDetails
            .FirstOrDefaultAsync(orderDetail => orderDetail.Id == id);
    }
}