namespace ReadersRealm.ViewModels.Book;

using Data.Models;
using ReadersRealm.Data.Models.Enums;

public class DeleteBookViewModel
{
    public Guid Id { get; set; }

    public required string Title { get; set; }

    public string? Description { get; set; }

    public required string ISBN { get; set; }

    public decimal Price { get; set; }

    public int? Pages { get; set; }

    public BookCover? BookCover { get; set; }

    public bool Used { get; set; }

    public Guid AuthorId { get; set; }

    public Author Author { get; set; }

    public int CategoryId { get; set; }

    public Category Category { get; set; }
}