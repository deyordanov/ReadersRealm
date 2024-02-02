namespace ReadersRealm.Data.Repositories.Contracts;

using Models;

public interface ICategoryRepository : IRepository<Category>
{
    Task<Category> GetByIdAsync(int id);
}