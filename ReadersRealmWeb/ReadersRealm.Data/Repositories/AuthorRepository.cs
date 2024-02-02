namespace ReadersRealm.Data.Repositories;

using Contracts;
using Microsoft.EntityFrameworkCore;
using Models;

public class AuthorRepository : Repository<Author>, IAuthorRepository
{
    private readonly ReadersRealmDbContext dbContext;
    public AuthorRepository(ReadersRealmDbContext dbContext) : base(dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<Author> GetByIdAsync(Guid id)
    {
        return await this
            .dbContext
            .Authors
            .FirstAsync(author => author.Id == id);
    }
}