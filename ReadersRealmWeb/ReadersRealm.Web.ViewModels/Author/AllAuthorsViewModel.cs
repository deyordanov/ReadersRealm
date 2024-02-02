namespace ReadersRealm.Web.ViewModels.Author;

using Data.Models;
using Data.Models.Enums;

public class AllAuthorsViewModel
{
    public AllAuthorsViewModel()
    {
        this.Books = new HashSet<Book>();
    }

    public Guid Id { get; set; }

    public required string FirstName { get; set; }

    public string? MiddleName { get; set; }

    public required string LastName { get; set; }

    public int? Age { get; set; }

    public Gender Gender { get; set; }

    public required string Email { get; set; }

    public required string PhoneNumber { get; set; }

    public HashSet<Book> Books { get; set; }
}