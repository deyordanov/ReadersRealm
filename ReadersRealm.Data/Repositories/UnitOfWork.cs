namespace ReadersRealm.Data.Repositories;

using Contracts;

public class UnitOfWork : IUnitOfWork
{
    private bool _disposed;
    private readonly ReadersRealmDbContext _dbContext;

    public UnitOfWork(ReadersRealmDbContext dbContext)
    {
        _disposed = false;
        _dbContext = dbContext;

        CategoryRepository = new CategoryRepository(dbContext);
        BookRepository = new BookRepository(dbContext);
        AuthorRepository = new AuthorRepository(dbContext);
        CompanyRepository = new CompanyRepository(dbContext);
        ShoppingCartRepository = new ShoppingCartRepository(dbContext);
        ApplicationUserRepository = new ApplicationUserRepository(dbContext);
        OrderHeaderRepository = new OrderHeaderRepository(dbContext);
        OrderDetailsRepository = new OrderDetailsRepository(dbContext);
        OrderRepository = new OrderRepository(dbContext);
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
        await _dbContext.SaveChangesAsync();
    }

    public void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _dbContext.Dispose();
            }
        }

        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}