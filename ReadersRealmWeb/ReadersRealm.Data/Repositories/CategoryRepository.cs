namespace ReadersRealm.Data.Repositories;

using Contracts;
using Models;

public class CategoryRepository : Repository<Category, int>
{
    public CategoryRepository(ReadersRealmDbContext dbContext) 
        : base(dbContext) { }
}