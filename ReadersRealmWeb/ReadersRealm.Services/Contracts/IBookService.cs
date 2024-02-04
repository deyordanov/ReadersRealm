namespace ReadersRealm.Services.Contracts;

using Common;
using Data.Models;
using ViewModels.Book;
using Web.ViewModels.Book;

public interface IBookService
{
    Task<PaginatedList<AllBooksViewModel>> GetAllAsync(int pageIndex, int pageSize);
    Task<Book?> GetBookByIdAsync(Guid id);
    Task<Book?> GetBookByIdWithNavPropertiesAsync(Guid id);
    Task<CreateBookViewModel> GetBookForCreateAsync();
    Task<EditBookViewModel> GetBookForEditAsync(Guid id);
    Task<DeleteBookViewModel> GetBookForDeleteAsync(Guid id);
    Task CreateBookAsync(CreateBookViewModel bookModel);
    Task EditBookAsync(EditBookViewModel bookModel);
    Task DeleteBookAsync(DeleteBookViewModel bookModel);
}