namespace ReadersRealm.Data.Repositories.Contracts;

using Models;

public interface IBookRepository : IRepository<Book>
{
    Task<Book> GetByIdAsync(Guid id);
}