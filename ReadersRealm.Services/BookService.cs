namespace ReadersRealm.Services;

using Common;
using Common.Exceptions;
using Contracts;
using Data.Models;
using Data.Repositories.Contracts;
using ReadersRealm.ViewModels.Author;
using ViewModels.Book;
using ViewModels.Category;

public class BookService : IBookService
{
    private const string PropertiesToInclude = "Author, Category";

    private readonly ICategoryService _categoryService;
    private readonly IAuthorService _authorService;
    private readonly IUnitOfWork _unitOfWork;

    public BookService(
        IUnitOfWork unitOfWork, 
        ICategoryService categoryService, 
        IAuthorService authorService)
    {
        this._unitOfWork = unitOfWork;
        this._categoryService = categoryService;
        this._authorService = authorService;
    }

    public async Task<PaginatedList<AllBooksViewModel>> GetAllAsync(int pageIndex, int pageSize, string? searchTerm)
    {
        List<Book> allBooks = await this
            ._unitOfWork
            .BookRepository
            .GetAsync(book => book.Title.ToLower().StartsWith(searchTerm != null ? searchTerm.ToLower() : string.Empty), null, PropertiesToInclude);

        return PaginatedList<AllBooksViewModel>.Create(allBooks
            .Select(book => new AllBooksViewModel()
            {
                Id = book.Id,
                ISBN = book.ISBN,
                Title = book.Title,
                Author = new AuthorViewModel()
                {
                    Id = book.Author.Id,
                    FirstName = book.Author.FirstName,
                    MiddleName = book.Author.MiddleName,
                    LastName = book.Author.LastName,
                    PhoneNumber = book.Author.PhoneNumber,
                    Email = book.Author.Email,
                    Age = book.Author.Age,
                    Gender = book.Author.Gender,
                },
                AuthorId = book.AuthorId,
                Category = new CategoryViewModel()
                {
                    Id = book.Category.Id,
                    Name = book.Category.Name,
                    DisplayOrder = book.Category.DisplayOrder,
                },
                CategoryId = book.CategoryId,
                BookCover = book.BookCover,
                Description = book.Description,
                Pages = book.Pages,
                Price = book.Price,
                Used = book.Used,
                ImageUrl = book.ImageUrl,
            })
            .ToList(), pageIndex, pageSize);
    }

    public async Task<EditBookViewModel> GetBookForEditAsync(Guid id)
    {
        Book? book =  await this
            ._unitOfWork
            .BookRepository
            .GetByIdAsync(id);

        if (book == null)
        {
            throw new BookNotFoundException();
        }

        List<AllAuthorsListViewModel> authorsList = await this
            ._authorService
            .GetAllListAsync();

        List<AllCategoriesListViewModel> categoriesList = await this
            ._categoryService
            .GetAllListAsync();

        EditBookViewModel bookModel = new EditBookViewModel()
        {
            Id = book.Id,
            ISBN = book.ISBN,
            Title = book.Title,
            BookCover = book.BookCover,
            Description = book.Description,
            Pages = book.Pages,
            Price = book.Price,
            Used = book.Used,
            ImageUrl = book.ImageUrl,
            AuthorId = book.AuthorId,
            CategoryId = book.CategoryId,
            AuthorsList = authorsList,
            CategoriesList = categoriesList
        };

        return bookModel;
    }

    public async Task<DeleteBookViewModel> GetBookForDeleteAsync(Guid id)
    {
        Book? book = await this
            ._unitOfWork
            .BookRepository
            .GetByIdWithNavPropertiesAsync(id, PropertiesToInclude);

        if (book == null)
        {
            throw new CategoryNotFoundException();
        }

        DeleteBookViewModel bookModel = new DeleteBookViewModel()
        {
            Id = book.Id,
            ISBN = book.ISBN,
            Title = book.Title,
            BookCover = book.BookCover,
            Description = book.Description,
            Pages = book.Pages,
            Price = book.Price,
            Used = book.Used,
            ImageUrl = book.ImageUrl,
            Author = new AuthorViewModel()
            {
                Id = book.Author.Id,
                FirstName = book.Author.FirstName,
                MiddleName = book.Author.MiddleName,
                LastName = book.Author.LastName,
                PhoneNumber = book.Author.PhoneNumber,
                Email = book.Author.Email,
                Age = book.Author.Age,
                Gender = book.Author.Gender,
            },
            AuthorId = book.AuthorId,
            Category = new CategoryViewModel()
            {
                Id = book.Category.Id,
                Name = book.Category.Name,
                DisplayOrder = book.Category.DisplayOrder,
            },
            CategoryId = book.CategoryId,
        };

        return bookModel;
    }

    public async Task<DetailsBookViewModel> GetBookForDetailsAsync(Guid id)
    {
        Book? book = await this
            ._unitOfWork
            .BookRepository
            .GetByIdWithNavPropertiesAsync(id, PropertiesToInclude);

        if (book == null)
        {
            throw new BookNotFoundException();
        }

        DetailsBookViewModel bookModel = new DetailsBookViewModel()
        {
            Id = book.Id,
            ISBN = book.ISBN,
            Title = book.Title,
            BookCover = book.BookCover,
            Description = book.Description,
            Pages = book.Pages,
            Price = book.Price,
            Used = book.Used,
            ImageUrl = book.ImageUrl,
            Author = new AuthorViewModel()
            {
                Id = book.Author.Id,
                FirstName = book.Author.FirstName,
                MiddleName = book.Author.MiddleName,
                LastName = book.Author.LastName,
                PhoneNumber = book.Author.PhoneNumber,
                Email = book.Author.Email,
                Age = book.Author.Age,
                Gender = book.Author.Gender,
            },
            AuthorId = book.AuthorId,
            Category = new CategoryViewModel()
            {
                Id = book.Category.Id,
                Name = book.Category.Name,
                DisplayOrder = book.Category.DisplayOrder,
            },
            CategoryId = book.CategoryId,
        };

        return bookModel;
    }

    public async Task<CreateBookViewModel> GetBookForCreateAsync()
    {
        List<AllAuthorsListViewModel> authorsList = await this
            ._authorService
            .GetAllListAsync();

        List<AllCategoriesListViewModel> categoriesList = await this
            ._categoryService
            .GetAllListAsync();

        CreateBookViewModel bookModel = new CreateBookViewModel()
        {
            Title = string.Empty,
            ISBN = string.Empty,
            AuthorsList = authorsList,
            CategoriesList = categoriesList
        };

        return bookModel;
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
            Description = bookModel.Description,
            Pages = bookModel.Pages,
            Price = bookModel.Price,
            Used = bookModel.Used,
            ImageUrl = bookModel.ImageUrl,
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
        bookToEdit.Description = bookModel.Description;
        bookToEdit.Pages = bookModel.Pages;
        bookToEdit.Price = bookModel.Price;
        bookToEdit.Used = bookModel.Used;
        bookToEdit.ImageUrl = bookModel.ImageUrl;

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