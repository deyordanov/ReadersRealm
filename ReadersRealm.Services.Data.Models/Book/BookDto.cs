// ReSharper disable InconsistentNaming
namespace ReadersRealm.Services.Data.Models.Book;

using Author;
using Category;
using ReadersRealm.Data.Models.Enums;

public class BookDto
{
    public Guid Id { get; set; }

    public required string Title { get; set; }

    public string? Description { get; set; }

    public required string ISBN { get; set; }

    public decimal Price { get; set; }

    public int? Pages { get; set; }

    public BookCover? BookCover { get; set; }

    public bool Used { get; set; }

    public string? ImageUrl { get; set; }

    public Guid AuthorId { get; set; }

    public AuthorDto Author { get; set; } = null!;

    public int CategoryId { get; set; }

    public CategoryDto Category { get; set; } = null!;
}