namespace ReadersRealm.Services;

using Common.Exceptions;
using Contracts;
using Data.Models;
using Data.Repositories.Contracts;
using ViewModels.Book;
using Web.ViewModels.Book;

public class BookService : IBookService
{
    private readonly IUnitOfWork unitOfWork;

    public BookService(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<AllBooksViewModel>> GetAllAsync()
    {
        List<Book> allBooks = await this
            .unitOfWork
            .BookRepository
            .GetAsync(null, null, "Author, Category");

        IEnumerable<AllBooksViewModel> booksToReturn = allBooks
            .Select(b => new AllBooksViewModel()
            {
                Id = b.Id,
                ISBN = b.ISBN,
                Title = b.Title,
                Author = b.Author,
                AuthorId = b.AuthorId,
                Category = b.Category,
                CategoryId = b.CategoryId,
                BookCover = b.BookCover,
                Description = b.Description,
                Pages = b.Pages,
                Price = b.Price,
                Used = b.Used,
            });

        return booksToReturn;
    }

    public async Task<Book?> GetBookByIdAsync(Guid id)
    {
        return await this
            .unitOfWork
            .BookRepository
            .GetByIdAsync(id);
    }

    public async Task<Book?> GetBookByIdWithNavPropertiesAsync(Guid id)
    {
        return await this
            .unitOfWork
            .BookRepository
            .GetByIdWithNavPropertiesAsync(id, "Author, Category");
    }

    public async Task CreateBookAsync(CreateBookViewModel bookModel)
    {
        Book bookToAdd = new Book()
        {
            Id = bookModel.Id,
            ISBN = bookModel.ISBN,
            Title = bookModel.Title,
            AuthorId = bookModel.AuthorId,
            CategoryId = bookModel.CategoryId,
            BookCover = bookModel.BookCover,
            Description = bookModel.Description,
            Pages = bookModel.Pages,
            Price = bookModel.Price,
            Used = bookModel.Used,
        };

        await this
            .unitOfWork
            .BookRepository
            .AddAsync(bookToAdd);

        await this
            .unitOfWork
            .SaveAsync();
    }

    public async Task EditBookAsync(EditBookViewModel bookModel)
    {
        Book? bookToEdit = await this
            .unitOfWork
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
        bookToEdit.Description = bookModel.Description;
        bookToEdit.Pages = bookModel.Pages;
        bookToEdit.Price = bookModel.Price;
        bookToEdit.Used = bookModel.Used;

        await this.unitOfWork.SaveAsync();
    }

    public async Task DeleteBookAsync(DeleteBookViewModel bookModel)
    {
        Book? bookToDelete = await this
            .unitOfWork
            .BookRepository
            .GetByIdAsync(bookModel.Id);

        if (bookToDelete == null)
        {
            throw new BookNotFoundException();
        }

        this.unitOfWork
            .BookRepository
            .Delete(bookToDelete);

        await this
            .unitOfWork
            .SaveAsync();
    }
}