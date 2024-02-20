namespace ReadersRealm.Data.Repositories;

using Common.Exceptions;
using Contracts;
using Microsoft.EntityFrameworkCore;
using Models;

public class BookRepository : Repository<Book>, IBookRepository
{
    private readonly ReadersRealmDbContext _dbContext;

    public BookRepository(ReadersRealmDbContext dbContext)
        : base(dbContext)
    {
        _dbContext = dbContext;
    }

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