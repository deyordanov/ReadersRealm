namespace ReadersRealm.Services.Data.AuthorServices;

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

    public async Task<bool> AuthorExistsAsync(Guid authorId)
    {
        return await this
            ._unitOfWork
            .AuthorRepository
            .GetFirstOrDefaultWithFilterAsync(author => author.Id.Equals(authorId), false) != null;
    }
}