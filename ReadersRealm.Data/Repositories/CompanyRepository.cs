namespace ReadersRealm.Data.Repositories;

using System.Linq.Expressions;
using Contracts;
using Microsoft.EntityFrameworkCore;
using Models;

public class CompanyRepository : Repository<Company>, ICompanyRepository
{
    private readonly ReadersRealmDbContext _dbContext;
    public CompanyRepository(ReadersRealmDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Company?> GetByIdAsync(Guid id)
    {
        return await _dbContext
            .Companies
            .FirstOrDefaultAsync(company => company.Id == id);
    }

    public async Task<Company?> GetFirstOrDefaultByFilterAsync(Expression<Func<Company, bool>> filter, bool tracking)
    {
        if (tracking)
        {
            return await this
                ._dbContext
                .Companies
                .FirstOrDefaultAsync(filter);
        }

        return await this
            ._dbContext
            .Companies
            .AsNoTracking()
            .FirstOrDefaultAsync(filter);
    }
}