namespace ReadersRealm.Data.Repositories;

using Contracts;
using Models;

public class UnitOfWork : IUnitOfWork
{
    private bool disposed;
    private ReadersRealmDbContext dbContext;

    public UnitOfWork(ReadersRealmDbContext dbContext)
    {
        this.disposed = false;
        this.dbContext = dbContext;

        this.CategoryRepository = new CategoryRepository(dbContext);
    }

    public CategoryRepository CategoryRepository { get; private set; }

    public async Task SaveAsync()
    {
        await this.dbContext.SaveChangesAsync();
    }

    public void Dispose(bool disposing)
    {
        if (!this.disposed)
        {
            if (disposing)
            {
                this.dbContext.Dispose();
            }
        }

        this.disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}