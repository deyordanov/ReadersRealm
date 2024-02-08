namespace ReadersRealm.Data.Repositories;

using Contracts;
using Microsoft.EntityFrameworkCore;
using Models;

public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
{
    private readonly ReadersRealmDbContext _dbContext;
    public OrderHeaderRepository(ReadersRealmDbContext dbContext) : base(dbContext)
    {
        this._dbContext = dbContext;
    }

    public async Task<OrderHeader?> GetByIdAsync(Guid id)
    {
        return await this
            ._dbContext
            .OrderHeaders
            .FirstOrDefaultAsync(orderHeader => orderHeader.Id == id);
    }
}