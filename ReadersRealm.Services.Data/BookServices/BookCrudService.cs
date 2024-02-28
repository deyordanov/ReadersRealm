namespace ReadersRealm.Services.Data.BookServices;

using System.Net;
using Common.Exceptions.Book;
using Contracts;
using Ganss.Xss;
using ReadersRealm.Data.Models;
using ReadersRealm.Data.Repositories.Contracts;
using Web.ViewModels.Book;

public class BookCrudService : IBookCrudService
{
    private readonly IHtmlSanitizer _sanitizer;
    private readonly IUnitOfWork _unitOfWork;

    public BookCrudService(IUnitOfWork unitOfWork)
    {
        this._sanitizer = new HtmlSanitizer();
        this._unitOfWork = unitOfWork;
    }

    public async Task CreateBookAsync(CreateBookViewModel bookModel)
    {
        Book bookToAdd = new Book()
        {
            ISBN = bookModel.ISBN,
            Title = bookModel.Title,
            AuthorId = bookModel.AuthorId,
            CategoryId = bookModel.CategoryId,
            BookCover = bookModel.BookCover,
            Description = this
                ._sanitizer
                .Sanitize(bookModel.Description ?? string.Empty),
            Pages = bookModel.Pages,
            Price = bookModel.Price,
            Used = bookModel.Used,
            ImageId = bookModel.ImageId,
        };

        await this
            ._unitOfWork
            .BookRepository
            .AddAsync(bookToAdd);

        await this
            ._unitOfWork
            .SaveAsync();
    }

    public async Task EditBookAsync(EditBookViewModel bookModel)
    {
        Book? bookToEdit = await this
            ._unitOfWork
            .BookRepository
            .GetByIdAsync(bookModel.Id);

        if (bookToEdit == null)
        {
            throw new BookNotFoundException();
        }

        bookToEdit.Id = bookModel.Id;
        bookToEdit.ISBN = bookModel.ISBN;
        bookToEdit.Title = bookModel.Title;
        bookToEdit.AuthorId = bookModel.AuthorId;
        bookToEdit.CategoryId = bookModel.CategoryId;
        bookToEdit.BookCover = bookModel.BookCover;
        bookToEdit.Description = this
            ._sanitizer
            .Sanitize(bookModel.Description ?? string.Empty);
        bookToEdit.Pages = bookModel.Pages;
        bookToEdit.Price = bookModel.Price;
        bookToEdit.Used = bookModel.Used;
        bookToEdit.ImageId = bookModel.ImageId;

        await this
            ._unitOfWork
            .SaveAsync();
    }

    public async Task DeleteBookAsync(DeleteBookViewModel bookModel)
    {
        Book? bookToDelete = await this
            ._unitOfWork
            .BookRepository
            .GetByIdAsync(bookModel.Id);

        if (bookToDelete == null)
        {
            throw new BookNotFoundException();
        }

        this._unitOfWork
            .BookRepository
            .Delete(bookToDelete);

        await this
            ._unitOfWork
            .SaveAsync();
    }
}