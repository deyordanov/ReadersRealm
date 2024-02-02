namespace ReadersRealm.Data.Repositories;

using Contracts;
using Microsoft.EntityFrameworkCore;
using Models;

public class AuthorBookRepository : Repository<AuthorBook>, IAuthorBookRepository
{
    private readonly ReadersRealmDbContext dbContext;
    public AuthorBookRepository(ReadersRealmDbContext dbContext) : base(dbContext)
    {
        this.dbContext = dbContext;
    }

    // public async Task<AuthorBook> GetByIdsAsync(Guid authorId, Guid bookId)
    // {
    //     return await this
    //         .dbContext
    //         .AuthorsBooks
    //         .FirstAsync(ab => ab.AuthorId == authorId && ab.BookId == bookId);
    // }
}