namespace ReadersRealm.Web.ViewModels.Book;

using Author;
using Category;
using Data.Models.Enums;

/// <summary>
/// A "generic" view model - it is used to transfer data in in other view models.
/// Example:
/// Author has a collection of books - this books will be a "generic" view model through which the navigation property data for the Author view model will be transferred.
/// </summary>
public class BookViewModel
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