namespace ReadersRealm.Services.Data.AuthorServices.Contracts;

using Web.ViewModels.Author;

public interface IAuthorRetrievalService
{
    Task<IEnumerable<AllAuthorsViewModel>> GetAllAsync();
    Task<List<AllAuthorsListViewModel>> GetAllListAsync();
    Task<bool> AuthorExistsAsync(Guid authorId);
}