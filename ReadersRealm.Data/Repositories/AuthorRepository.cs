namespace ReadersRealm.Data.Repositories;

using System.Linq.Expressions;
using Contracts;
using Microsoft.EntityFrameworkCore;
using Models;

public class AuthorRepository(ReadersRealmDbContext dbContext) : Repository<Author>(dbContext), IAuthorRepository
{
    private readonly ReadersRealmDbContext _dbContext = dbContext;

    public async Task<Author?> GetByIdAsync(Guid id)
    {
        return await this
            ._dbContext
            .Authors
            .FirstAsync(author => author.Id == id);
    }

    public async Task<Author?> GetFirstOrDefaultWithFilterAsync(Expression<Func<Author, bool>> filter, bool tracking)
    {
        if (tracking)
        {
            return await this
                ._dbContext
                .Authors
                .FirstOrDefaultAsync(filter);
        }

        return await this
            ._dbContext
            .Authors
            .AsNoTracking()
            .FirstOrDefaultAsync(filter);
    }
}