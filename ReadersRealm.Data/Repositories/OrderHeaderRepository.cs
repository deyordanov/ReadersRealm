namespace ReadersRealm.Data.Repositories;

using Common.Exceptions.GeneralExceptions;
using Contracts;
using Microsoft.EntityFrameworkCore;
using Models;

public class OrderHeaderRepository(ReadersRealmDbContext dbContext)
    : Repository<OrderHeader>(dbContext), IOrderHeaderRepository
{
    private readonly ReadersRealmDbContext _dbContext = dbContext;

    public async Task<OrderHeader?> GetByIdAsync(Guid id)
    {
        return await _dbContext
            .OrderHeaders
            .FirstOrDefaultAsync(orderHeader => orderHeader.Id == id);
    }

    public Task<OrderHeader?> GetByIdWithNavPropertiesAsync(Guid id, string properties)
    {
        IQueryable<OrderHeader> query = _dbContext.OrderHeaders;

        string[] propertiesToAdd = properties.Split(", ", StringSplitOptions.RemoveEmptyEntries);

        if (!ArePropertiesPresentInEntity(propertiesToAdd))
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