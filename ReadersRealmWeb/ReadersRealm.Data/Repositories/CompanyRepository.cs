namespace ReadersRealm.Data.Repositories;

using Contracts;
using Microsoft.EntityFrameworkCore;
using Models;

public class CompanyRepository : Repository<Company>, ICompanyRepository
{
    private readonly ReadersRealmDbContext _dbContext;
    public CompanyRepository(ReadersRealmDbContext dbContext) : base(dbContext)
    {
        this._dbContext = dbContext;
    }

    public async Task<Company?> GetByIdAsync(Guid id)
    {
        return await this
            ._dbContext
            .Companies
            .FirstOrDefaultAsync(company => company.Id == id);
    }
}