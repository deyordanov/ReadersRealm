namespace ReadersRealm.Services.Data.AuthorServices;

using Common.Exceptions.Author;
using Contracts;
using ReadersRealm.Data.Models;
using ReadersRealm.Data.Repositories.Contracts;
using Web.ViewModels.Author;

public class AuthorCrudService(IUnitOfWork unitOfWork) : IAuthorCrudService
{
    public async Task CreateAuthorAsync(CreateAuthorViewModel authorModel)
    {
        Author author = new Author()
        {
            FirstName = authorModel.FirstName,
            MiddleName = authorModel.MiddleName,
            LastName = authorModel.LastName,
            Email = authorModel.Email,
            PhoneNumber = authorModel.PhoneNumber,
            Age = authorModel.Age,
            Gender = authorModel.Gender,
        };

        await unitOfWork
            .AuthorRepository
            .AddAsync(author);

        await unitOfWork
            .SaveAsync();
    }

    public async Task EditAuthorAsync(EditAuthorViewModel authorModel)
    {
        Author? author = await unitOfWork
            .AuthorRepository
            .GetByIdAsync(authorModel.Id);

        if (author == null)
        {
            throw new AuthorNotFoundException();
        }

        author.Id = authorModel.Id;
        author.FirstName = authorModel.FirstName;
        author.MiddleName = authorModel.MiddleName;
        author.LastName = authorModel.LastName;
        author.Age = authorModel.Age;
        author.Gender = authorModel.Gender;
        author.Email = authorModel.Email;
        author.PhoneNumber = authorModel.PhoneNumber;

        await unitOfWork
            .SaveAsync();
    }

    public async Task DeleteAuthorAsync(DeleteAuthorViewModel authorModel)
    {
        Author? author = await unitOfWork
            . AuthorRepository
            .GetByIdAsync(authorModel.Id);

        if (author == null)
        {
            throw new AuthorNotFoundException();
        }

        unitOfWork
            .AuthorRepository
            .Delete(author);

        await unitOfWork
            .SaveAsync();
    }
}