namespace ReadersRealm.Web.ViewModels.Author;

using Book;
using Data.Models.Enums;

public class AuthorViewModel
{
    public AuthorViewModel()
    {
        Books = new HashSet<BookViewModel>();
    }

    public Guid Id { get; set; }

    public required string FirstName { get; set; }

    public string? MiddleName { get; set; }

    public required string LastName { get; set; }

    public int? Age { get; set; }

    public Gender Gender { get; set; }

    public required string Email { get; set; }

    public required string PhoneNumber { get; set; }

    public HashSet<BookViewModel> Books { get; set; }
}