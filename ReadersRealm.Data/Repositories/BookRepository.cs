namespace ReadersRealm.Data.Repositories;

using Common.Exceptions.GeneralExceptions;
using Contracts;
using Microsoft.EntityFrameworkCore;
using Models;

public class BookRepository(ReadersRealmDbContext dbContext) : Repository<Book>(dbContext), IBookRepository
{
    private readonly ReadersRealmDbContext _dbContext = dbContext;

    public async Task<Book?> GetByIdAsync(Guid id)
    {
        return await _dbContext
            .Books
            .FirstOrDefaultAsync(book => book.Id == id);
    }

    public Task<Book?> GetByIdWithNavPropertiesAsync(Guid id, string properties)
    {
        IQueryable<Book> query = _dbContext.Books.AsNoTracking();

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