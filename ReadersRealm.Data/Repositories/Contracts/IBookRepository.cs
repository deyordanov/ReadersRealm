namespace ReadersRealm.Data.Repositories.Contracts;

using Models;

public interface IBookRepository : IRepository<Book>
{
    Task<Book?> GetByIdAsync(Guid id);
    Task<Book?> GetByIdWithNavPropertiesAsync(Guid id, string properties);
}