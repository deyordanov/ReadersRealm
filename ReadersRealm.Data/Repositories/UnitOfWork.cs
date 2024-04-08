namespace ReadersRealm.Data.Repositories;

using Contracts;

public class UnitOfWork(ReadersRealmDbContext dbContext) : IUnitOfWork
{
    private bool _disposed = false;

    public ICategoryRepository CategoryRepository { get; private set; } = new CategoryRepository(dbContext);
    public IBookRepository BookRepository { get; private set; } = new BookRepository(dbContext);
    public IAuthorRepository AuthorRepository { get; private set; } = new AuthorRepository(dbContext);
    public ICompanyRepository CompanyRepository { get; private set; } = new CompanyRepository(dbContext);
    public IShoppingCartRepository ShoppingCartRepository { get; private set; } = new ShoppingCartRepository(dbContext);
    public IApplicationUserRepository ApplicationUserRepository { get; private set; } = new ApplicationUserRepository(dbContext);
    public IOrderHeaderRepository OrderHeaderRepository { get; private set; } = new OrderHeaderRepository(dbContext);
    public IOrderDetailsRepository OrderDetailsRepository { get; } = new OrderDetailsRepository(dbContext);
    public IOrderRepository OrderRepository { get; } = new OrderRepository(dbContext);

    public async Task SaveAsync()
    {
        await dbContext.SaveChangesAsync();
    }

    public void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                dbContext.Dispose();
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