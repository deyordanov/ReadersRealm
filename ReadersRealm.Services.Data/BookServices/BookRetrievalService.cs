namespace ReadersRealm.Services.Data.BookServices;

using AuthorServices.Contracts;
using CategoryServices.Contracts;
using Common;
using Common.Exceptions.Book;
using Common.Exceptions.Category;
using Contracts;
using ReadersRealm.Data.Models;
using ReadersRealm.Data.Repositories.Contracts;
using Web.ViewModels.Author;
using Web.ViewModels.Book;
using Web.ViewModels.Category;

public class BookRetrievalService : IBookRetrievalService
{
    private const string PropertiesToInclude = "Author, Category";

    private readonly ICategoryRetrievalService _categoryRetrievalService;
    private readonly IAuthorRetrievalService _authorRetrievalService;
    private readonly IUnitOfWork _unitOfWork;

    public BookRetrievalService(
        IUnitOfWork unitOfWork,
        IAuthorRetrievalService authorRetrievalService, 
        ICategoryRetrievalService categoryRetrievalService)
    {
        this._unitOfWork = unitOfWork;
        this._authorRetrievalService = authorRetrievalService;
        this._categoryRetrievalService = categoryRetrievalService;
    }

    public async Task<PaginatedList<AllBooksViewModel>> GetAllAsync(int pageIndex, int pageSize, string? searchTerm)
    {
        List<Book> allBooks = await this
            ._unitOfWork
            .BookRepository
            .GetAsync(book => book
                .Title
                .ToLower()
                .StartsWith(searchTerm != null ? searchTerm.ToLower() : string.Empty), 
                null, 
                PropertiesToInclude);

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
        Book? book = await this
            ._unitOfWork
            .BookRepository
            .GetByIdAsync(id);

        if (book == null)
        {
            throw new BookNotFoundException();
        }

        List<AllAuthorsListViewModel> authorsList = await this
            ._authorRetrievalService
            .GetAllListAsync();

        List<AllCategoriesListViewModel> categoriesList = await this
            ._categoryRetrievalService
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
            ._authorRetrievalService
            .GetAllListAsync();

        List<AllCategoriesListViewModel> categoriesList = await this
            ._categoryRetrievalService
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
}