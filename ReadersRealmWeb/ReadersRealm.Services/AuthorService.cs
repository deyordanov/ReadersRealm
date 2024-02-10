namespace ReadersRealm.Services;

using Contracts;
using Data.Models;
using Data.Repositories.Contracts;
using ViewModels.Author;
using ViewModels.Book;
using static Common.Constants.ValidationConstants.Author;

public class AuthorService : IAuthorService
{
    private readonly IUnitOfWork _unitOfWork;

    public AuthorService(IUnitOfWork unitOfWork)
    {
        this._unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<AllAuthorsViewModel>> GetAllAsync()
    {
        List<Author> allAuthors = await this
            ._unitOfWork
            .AuthorRepository
            .GetAsync(null, null, string.Empty);

        IEnumerable<AllAuthorsViewModel> authorsToReturn = allAuthors
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
            });

        return authorsToReturn;
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
}