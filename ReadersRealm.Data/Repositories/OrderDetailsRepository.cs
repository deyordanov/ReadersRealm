namespace ReadersRealm.Data.Repositories;

using Contracts;
using Microsoft.EntityFrameworkCore;
using Models;

public class OrderDetailsRepository(ReadersRealmDbContext dbContext)
    : Repository<OrderDetails>(dbContext), IOrderDetailsRepository
{
    private readonly ReadersRealmDbContext _dbContext = dbContext;

    public async Task<OrderDetails?> GetByIdAsync(Guid id)
    {
        return await _dbContext
            .OrdersDetails
            .FirstOrDefaultAsync(orderDetail => orderDetail.Id == id);
    }
}