namespace ReadersRealm.Data.Repositories;

using Contracts;

public class UnitOfWork : IUnitOfWork
{
    private bool _disposed;
    private readonly ReadersRealmDbContext _dbContext;

    public UnitOfWork(ReadersRealmDbContext dbContext)
    {
        this._disposed = false;
        this._dbContext = dbContext;

        this.CategoryRepository = new CategoryRepository(dbContext);
        this.BookRepository = new BookRepository(dbContext);
        this.AuthorRepository = new AuthorRepository(dbContext);
        this.CompanyRepository = new CompanyRepository(dbContext);
        this.ShoppingCartRepository = new ShoppingCartRepository(dbContext);
        this.ApplicationUserRepository = new ApplicationUserRepository(dbContext);
        this.OrderHeaderRepository = new OrderHeaderRepository(dbContext);
        this.OrderDetailsRepository = new OrderDetailsRepository(dbContext);
        this.OrderRepository = new OrderRepository(dbContext);
    }

    public ICategoryRepository CategoryRepository { get; private set; }
    public IBookRepository BookRepository { get; private set; }
    public IAuthorRepository AuthorRepository { get; private set; }
    public ICompanyRepository CompanyRepository { get; private set; }
    public IShoppingCartRepository ShoppingCartRepository { get; private set; }
    public IApplicationUserRepository ApplicationUserRepository { get; private set; }
    public IOrderHeaderRepository OrderHeaderRepository { get; private set; }
    public IOrderDetailsRepository OrderDetailsRepository { get; }
    public IOrderRepository OrderRepository { get; }

    public async Task SaveAsync()
    {
        await this._dbContext.SaveChangesAsync();
    }

    public void Dispose(bool disposing)
    {
        if (!this._disposed)
        {
            if (disposing)
            {
                this._dbContext.Dispose();
            }
        }

        this._disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}