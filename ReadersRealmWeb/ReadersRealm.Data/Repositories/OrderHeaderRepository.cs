namespace ReadersRealm.Data.Repositories;

using Contracts;
using Microsoft.EntityFrameworkCore;
using Models;

public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
{
    private readonly ReadersRealmDbContext dbContext;
    public OrderHeaderRepository(ReadersRealmDbContext dbContext) : base(dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<OrderHeader?> GetByIdAsync(Guid id)
    {
        return await this
            .dbContext
            .OrderHeaders
            .FirstOrDefaultAsync(orderHeader => orderHeader.Id == id);
    }
}