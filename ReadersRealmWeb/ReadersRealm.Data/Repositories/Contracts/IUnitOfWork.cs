namespace ReadersRealm.Data.Repositories.Contracts;

using Models;

public interface IUnitOfWork : IDisposable
{
    ICategoryRepository CategoryRepository { get; }
    IBookRepository BookRepository { get; }
    IAuthorRepository AuthorRepository { get; }
    ICompanyRepository CompanyRepository { get; }
    IShoppingCartRepository ShoppingCartRepository { get; }
    IApplicationUserRepository ApplicationUserRepository { get; }
    Task SaveAsync();
    void Dispose(bool disposing);
}