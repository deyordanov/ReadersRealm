﻿namespace ReadersRealm.Services.Contracts;

using Data.Models;
using ViewModels.Book;
using Web.ViewModels.Book;

public interface IBookService
{
    Task<IEnumerable<AllBooksViewModel>> GetAllAsync();
    Task<Book?> GetBookByIdAsync(Guid id);
    Task<Book?> GetBookByIdWithNavPropertiesAsync(Guid id);
    Task CreateBookAsync(CreateBookViewModel bookModel);
    Task EditBookAsync(EditBookViewModel bookModel);
    Task DeleteBookAsync(DeleteBookViewModel bookModel);
}