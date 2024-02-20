﻿namespace ReadersRealm.Services.Contracts;

using Common;
using Data.Models;
using ViewModels.Book;

public interface IBookService
{
    Task<PaginatedList<AllBooksViewModel>> GetAllAsync(int pageIndex, int pageSize, string? searchTerm);
    Task<CreateBookViewModel> GetBookForCreateAsync();
    Task<EditBookViewModel> GetBookForEditAsync(Guid id);
    Task<DeleteBookViewModel> GetBookForDeleteAsync(Guid id);
    Task<DetailsBookViewModel> GetBookForDetailsAsync(Guid id);
    Task CreateBookAsync(CreateBookViewModel bookModel);
    Task EditBookAsync(EditBookViewModel bookModel);
    Task DeleteBookAsync(DeleteBookViewModel bookModel);
}