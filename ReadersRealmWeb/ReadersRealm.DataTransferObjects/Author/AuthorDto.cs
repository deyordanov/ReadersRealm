namespace ReadersRealm.DataTransferObjects.Author;

using Book;
using Data.Models.Enums;

public class AuthorDto
{
    public AuthorDto()
    {
        this.Books = new HashSet<BookDto>();
    }

    public Guid Id { get; set; }

    public required string FirstName { get; set; }

    public string? MiddleName { get; set; }

    public required string LastName { get; set; }

    public int? Age { get; set; }

    public Gender Gender { get; set; }

    public required string Email { get; set; }

    public required string PhoneNumber { get; set; }

    public HashSet<BookDto> Books { get; set; }
}