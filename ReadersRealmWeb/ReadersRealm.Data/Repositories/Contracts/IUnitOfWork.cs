namespace ReadersRealm.Data.Repositories.Contracts;

using Models;

public interface IUnitOfWork : IDisposable
{
    ICategoryRepository CategoryRepository { get; }
    IBookRepository BookRepository { get; }
    IAuthorRepository AuthorRepository { get; }
    ICompanyRepository CompanyRepository { get; }
    Task SaveAsync();
    void Dispose(bool disposing);
}