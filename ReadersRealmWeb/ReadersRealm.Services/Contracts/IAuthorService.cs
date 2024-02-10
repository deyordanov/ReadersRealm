namespace ReadersRealm.Services.Contracts;

using ViewModels.Author;

public interface IAuthorService
{
    Task<IEnumerable<AllAuthorsViewModel>> GetAllAsync();
    Task<List<AllAuthorsListViewModel>> GetAllListAsync();
}