namespace ReadersRealm.Data.Repositories;

using Common.Exceptions;
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

    public Task<OrderHeader?> GetByIdWithNavPropertiesAsync(Guid id, string properties)
    {
        IQueryable<OrderHeader> query = this._dbContext.OrderHeaders;

        string[] propertiesToAdd = properties.Split(", ", StringSplitOptions.RemoveEmptyEntries);

        if (!this.ArePropertiesPresentInEntity(propertiesToAdd))
        {
            throw new PropertyNotFoundException();
        }

        foreach (string property in propertiesToAdd)
        {
            query = query.Include(property);
        }

        return query.FirstOrDefaultAsync(b => b.Id == id);
    }
}