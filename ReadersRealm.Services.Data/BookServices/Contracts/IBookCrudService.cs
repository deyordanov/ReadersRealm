namespace ReadersRealm.Services.Data.BookServices.Contracts;

using Web.ViewModels.Book;

public interface IBookCrudService
{
    Task CreateBookAsync(CreateBookViewModel bookModel);
    Task EditBookAsync(EditBookViewModel bookModel);
    Task DeleteBookAsync(DeleteBookViewModel bookModel);
}