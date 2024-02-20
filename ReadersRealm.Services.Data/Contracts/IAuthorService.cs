namespace ReadersRealm.Services.Data.Contracts;

using ReadersRealm.ViewModels.Author;

public interface IAuthorService
{
    Task<IEnumerable<AllAuthorsViewModel>> GetAllAsync();
    Task<List<AllAuthorsListViewModel>> GetAllListAsync();
}