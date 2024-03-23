namespace ReadersRealm.Services.Data.AuthorServices;

using Common;
using Common.Exceptions.Author;
using Contracts;
using ReadersRealm.Data.Models;
using ReadersRealm.Data.Repositories.Contracts;
using Web.ViewModels.Author;
using Web.ViewModels.Book;

public class AuthorRetrievalService : IAuthorRetrievalService
{
    private readonly IUnitOfWork _unitOfWork;

    public AuthorRetrievalService(IUnitOfWork unitOfWork)
    {
        this._unitOfWork = unitOfWork;
    }

    public async Task<PaginatedList<AllAuthorsViewModel>> GetAllAsync(int pageIndex, int pageSize, string? searchTerm)
    {
        List<Author> allAuthors = await this
            ._unitOfWork
            .AuthorRepository
            .GetAsync(author => author
                .FirstName
                .ToLower()
                .StartsWith(searchTerm != null ? searchTerm.ToLower() : string.Empty) ||
                author
                .LastName
                .ToLower()
                .StartsWith(searchTerm != null ? searchTerm.ToLower() : string.Empty), null, string.Empty);



        return PaginatedList<AllAuthorsViewModel>.Create(allAuthors
            .Select(author => new AllAuthorsViewModel()
            {
                LastName = author.LastName,
                Email = author.Email,
                FirstName = author.FirstName,
                PhoneNumber = author.PhoneNumber,
                Age = author.Age,
                Books = author.Books.Select(book => new BookViewModel()
                {
                    ISBN = book.ISBN,
                    Title = book.Title,
                    AuthorId = book.AuthorId,
                    BookCover = book.BookCover,
                    CategoryId = book.CategoryId,
                    Description = book.Description,
                    Id = book.Id,
                    Pages = book.Pages,
                    Price = book.Price,
                    Used = book.Used,
                }),
                Gender = author.Gender,
                Id = author.Id,
                MiddleName = author.MiddleName,
            })
            .ToList(), 
            pageIndex, 
            pageSize);
    }

    public async Task<List<AllAuthorsListViewModel>> GetAllListAsync()
    {
        List<Author> allAuthors = await this
            ._unitOfWork
            .AuthorRepository
            .GetAsync(null, null, string.Empty);

        List<AllAuthorsListViewModel> authorsToReturn = allAuthors
            .Select(a => new AllAuthorsListViewModel()
            {
                Id = a.Id,
                FirstName = a.FirstName,
                LastName = a.LastName,
            })
            .ToList();

        return authorsToReturn;
    }

    public CreateAuthorViewModel GetAuthorForCreate()
    {
        return new CreateAuthorViewModel()
        {
            FirstName = string.Empty,
            LastName = string.Empty,
            Email = string.Empty,
            PhoneNumber = string.Empty,
        };
    }

    public async Task<EditAuthorViewModel> GetAuthorForEditAsync(Guid id)
    {
        Author? author = await this
            ._unitOfWork
            .AuthorRepository
            .GetByIdAsync(id);

        if (author == null)
        {
            throw new AuthorNotFoundException();
        }

        EditAuthorViewModel authorModel = new EditAuthorViewModel()
        {
            Id = id,
            FirstName = author.FirstName,
            MiddleName = author.MiddleName,
            LastName = author.LastName,
            Email = author.Email,
            PhoneNumber = author.PhoneNumber,
            Gender = author.Gender,
            Age = author.Age,
        };

        return authorModel;
    }

    public async Task<DeleteAuthorViewModel> GetAuthorForDeleteAsync(Guid id)
    {
        Author? author = await this
            ._unitOfWork
            .AuthorRepository
            .GetByIdAsync(id);

        if (author == null)
        {
            throw new AuthorNotFoundException();
        }

        DeleteAuthorViewModel authorModel = new DeleteAuthorViewModel()
        {
            Id = author.Id,
            FirstName = author.FirstName,
            MiddleName = author.MiddleName,
            LastName = author.LastName,
            Email = author.Email,
            PhoneNumber = author.PhoneNumber,
            Gender = author.Gender,
            Age = author.Age,
        };

        return authorModel;
    }

    public async Task<bool> AuthorExistsAsync(Guid authorId)
    {
        return await this
            ._unitOfWork
            .AuthorRepository
            .GetFirstOrDefaultWithFilterAsync(author => author.Id.Equals(authorId), false) != null;
    }
}