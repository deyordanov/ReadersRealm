namespace ReadersRealm.Data.Repositories;

using Contracts;
using Microsoft.EntityFrameworkCore;
using Models;

public class OrderDetailsRepository : Repository<OrderDetails>, IOrderDetailsRepository
{
    private readonly ReadersRealmDbContext dbContext;
    public OrderDetailsRepository(ReadersRealmDbContext dbContext) : base(dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<OrderDetails?> GetByIdAsync(Guid id)
    {
        return await this
            .dbContext
            .OrdersDetails
            .FirstOrDefaultAsync(orderDetail => orderDetail.Id == id);
    }
}