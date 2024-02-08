namespace ReadersRealm.Data.Repositories;

using Contracts;
using Microsoft.AspNetCore.Identity;
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
        this.BookRepository = new BookRepository(dbContext);
        this.AuthorRepository = new AuthorRepository(dbContext);
        this.CompanyRepository = new CompanyRepository(dbContext);
        this.ShoppingCartRepository = new ShoppingCartRepository(dbContext);
        this.ApplicationUserRepository = new ApplicationUserRepository(dbContext);
        this.OrderHeaderRepository = new OrderHeaderRepository(dbContext);
        this.OrderDetailsRepository = new OrderDetailsRepository(dbContext);
    }

    public ICategoryRepository CategoryRepository { get; private set; }

    public IBookRepository BookRepository { get; private set; }

    public IAuthorRepository AuthorRepository { get; private set; }

    public ICompanyRepository CompanyRepository { get; private set; }
    public IShoppingCartRepository ShoppingCartRepository { get; private set; }
    public IApplicationUserRepository ApplicationUserRepository { get; private set; }
    public IOrderHeaderRepository OrderHeaderRepository { get; private set; }
    public IOrderDetailsRepository OrderDetailsRepository { get; }

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