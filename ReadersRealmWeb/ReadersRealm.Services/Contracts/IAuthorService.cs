namespace ReadersRealm.Services.Contracts;

using Web.ViewModels.Author;

public interface IAuthorService
{
    Task<IEnumerable<AllAuthorsViewModel>> GetAllAsync();

    Task<List<AllAuthorsListViewModel>> GetAllListAsync();
}