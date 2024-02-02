namespace ReadersRealm.Data.Repositories.Contracts;

using Models;

public interface IAuthorBookRepository : IRepository<AuthorBook>
{
    Task<AuthorBook> GetByIdsAsync(Guid authorId, Guid bookId);
}