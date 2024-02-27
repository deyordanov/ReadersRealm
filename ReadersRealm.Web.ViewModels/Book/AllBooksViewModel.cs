// ReSharper disable InconsistentNaming
namespace ReadersRealm.Web.ViewModels.Book;

using Author;
using Category;
using Data.Models.Enums;

public class AllBooksViewModel
{
    public Guid Id { get; set; }

    public required string Title { get; set; }

    public string? Description { get; set; }

    public required string ISBN { get; set; }

    public decimal Price { get; set; }

    public int? Pages { get; set; }

    public BookCover? BookCover { get; set; }

    public bool Used { get; set; }

    public string? ImageId { get; set; }

    public Guid AuthorId { get; set; }

    public AuthorViewModel Author { get; set; } = null!;

    public int CategoryId { get; set; }

    public CategoryViewModel Category { get; set; } = null!;
}