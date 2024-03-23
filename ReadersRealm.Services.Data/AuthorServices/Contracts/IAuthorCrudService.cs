namespace ReadersRealm.Services.Data.AuthorServices.Contracts;

using Web.ViewModels.Author;

public interface IAuthorCrudService
{
    Task CreateAuthorAsync(CreateAuthorViewModel authorModel);
    Task EditAuthorAsync(EditAuthorViewModel authorModel);
    Task DeleteAuthorAsync(DeleteAuthorViewModel authorModel);
}