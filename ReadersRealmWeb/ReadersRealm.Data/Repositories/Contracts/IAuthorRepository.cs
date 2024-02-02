namespace ReadersRealm.Data.Repositories.Contracts;

using Models;

public interface IAuthorRepository : IRepository<Author>
{
    Task<Author> GetByIdAsync(Guid id);
}