namespace ReadersRealm.Data.Repositories;

using Contracts;
using Microsoft.EntityFrameworkCore;
using Models;

public class BookRepository : Repository<Book>, IBookRepository
{
    private readonly ReadersRealmDbContext dbContext;

    public BookRepository(ReadersRealmDbContext dbContext)
        : base(dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<Book> GetByIdAsync(Guid id)
    {
        return await this
            .dbContext
            .Books
            .FirstAsync(book => book.Id == id);
    }
}