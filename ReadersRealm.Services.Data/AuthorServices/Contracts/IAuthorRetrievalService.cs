namespace ReadersRealm.Services.Data.AuthorServices.Contracts;

using Common;
using Web.ViewModels.Author;

public interface IAuthorRetrievalService
{
    Task<PaginatedList<AllAuthorsViewModel>> GetAllAsync(int pageIndex, int pageSize, string? searchTerm);
    Task<List<AllAuthorsListViewModel>> GetAllListAsync();
    CreateAuthorViewModel GetAuthorForCreate();
    Task<EditAuthorViewModel> GetAuthorForEditAsync(Guid id);
    Task<DeleteAuthorViewModel> GetAuthorForDeleteAsync(Guid id);
    Task<bool> AuthorExistsAsync(Guid authorId);
}