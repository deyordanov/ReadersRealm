﻿namespace ReadersRealm.Data.Repositories.Contracts;

using Models;

public interface IUnitOfWork : IDisposable
{
    ICategoryRepository CategoryRepository { get; }
    IBookRepository BookRepository { get; }
    IAuthorRepository AuthorRepository { get; }
    ICompanyRepository CompanyRepository { get; }
    IShoppingCartRepository ShoppingCartRepository { get; }
    IApplicationUserRepository ApplicationUserRepository { get; }
    IOrderHeaderRepository OrderHeaderRepository { get; }
    IOrderDetailsRepository OrderDetailsRepository { get; }
    Task SaveAsync();
    void Dispose(bool disposing);
}