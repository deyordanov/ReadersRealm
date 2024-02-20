namespace ReadersRealm.Data.Repositories;

using Common.Exceptions;
using Contracts;
using Microsoft.EntityFrameworkCore;
using Models;

public class OrderRepository : Repository<Order>, IOrderRepository
{
    private readonly ReadersRealmDbContext _dbContext;

    public OrderRepository(ReadersRealmDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Order?> GetByIdAsync(Guid id)
    {
        return await _dbContext
            .Orders
            .FirstOrDefaultAsync(order => order.Id == id);
    }

    public Task<Order?> GetByIdWithNavPropertiesAsync(Guid id, string properties)
    {
        IQueryable<Order> query = _dbContext.Orders;

        string[] propertiesToAdd = properties.Split(", ", StringSplitOptions.RemoveEmptyEntries);

        if (!ArePropertiesPresentInEntity(propertiesToAdd))
        {
            throw new PropertyNotFoundException();
        }

        foreach (string property in propertiesToAdd)
        {
            query = query.Include(property);
        }

        return query.FirstOrDefaultAsync(order => order.Id == id);
    }

    public async Task<Order?> GetByOrderHeaderIdAsync(Guid orderHeaderId)
    {
        return await _dbContext
            .Orders
            .FirstOrDefaultAsync(order => order.OrderHeaderId == orderHeaderId);
    }
}