﻿namespace ReadersRealm.Data.Repositories;

using Common.Exceptions;
using Contracts;
using Microsoft.EntityFrameworkCore;
using Models;
using System.Linq.Expressions;

public class ShoppingCartRepository : Repository<ShoppingCart>, IShoppingCartRepository
{
    private readonly ReadersRealmDbContext _dbContext;
    public ShoppingCartRepository(ReadersRealmDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ShoppingCart?> GetByIdAsync(Guid id)
    {
        return await _dbContext
            .ShoppingCarts
            .FirstOrDefaultAsync(shoppingCart => shoppingCart.Id == id);
    }

    public Task<ShoppingCart?> GetByIdWithNavPropertiesAsync(Guid id, string properties)
    {
        IQueryable<ShoppingCart> query = _dbContext.ShoppingCarts;

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

    public Task<ShoppingCart?> GetFirstOrDefaultWithFilterAsync(Expression<Func<ShoppingCart, bool>> filter)
    {
        return _dbContext
            .ShoppingCarts
            .AsNoTracking()
            .FirstOrDefaultAsync(filter);
    }

    public Task<ShoppingCart?> GetByApplicationUserIdAndBookIdAsync(string applicationUserId, Guid bookId)
    {
        return _dbContext
            .ShoppingCarts
            .FirstOrDefaultAsync(shoppingCart => shoppingCart.ApplicationUserId == applicationUserId &&
                                                 shoppingCart.BookId == bookId);
    }

    public async Task<List<ShoppingCart>> GetAllByApplicationUserIdAsync(string applicationUserId)
    {
        return await _dbContext
            .ShoppingCarts
            .Where(shoppingCart => shoppingCart.ApplicationUserId == applicationUserId)
            .ToListAsync();
    }
}