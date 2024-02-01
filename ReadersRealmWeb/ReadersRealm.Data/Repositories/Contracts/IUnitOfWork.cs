namespace ReadersRealm.Data.Repositories.Contracts;

using Models;

public interface IUnitOfWork : IDisposable
{
    CategoryRepository CategoryRepository { get; }
    Task SaveAsync();
    void Dispose(bool disposing);
}