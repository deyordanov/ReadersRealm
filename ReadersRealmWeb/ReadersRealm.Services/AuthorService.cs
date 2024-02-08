namespace ReadersRealm.Services;

using Contracts;
using Data.Models;
using Data.Repositories.Contracts;
using Web.ViewModels.Author;

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
            .Select(a => new AllAuthorsViewModel()
            {
                LastName = a.LastName,
                Email = a.Email,
                FirstName = a.FirstName,
                PhoneNumber = a.PhoneNumber,
                Age = a.Age,
                Books = a.Books,
                Gender = a.Gender,
                Id = a.Id,
                MiddleName = a.MiddleName,
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