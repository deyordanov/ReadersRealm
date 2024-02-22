namespace ReadersRealm.Services.Data.BookServices.Contracts;

using Common;
using Web.ViewModels.Book;

public interface IBookRetrievalService
{
    Task<PaginatedList<AllBooksViewModel>> GetAllAsync(int pageIndex, int pageSize, string? searchTerm);
    Task<CreateBookViewModel> GetBookForCreateAsync();
    Task<EditBookViewModel> GetBookForEditAsync(Guid id);
    Task<DeleteBookViewModel> GetBookForDeleteAsync(Guid id);
    Task<DetailsBookViewModel> GetBookForDetailsAsync(Guid id);
}