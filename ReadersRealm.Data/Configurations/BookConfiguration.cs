namespace ReadersRealm.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;
using ReadersRealm.Data.Models.Enums;

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.HasData(GenerateBooks());
    }

    public static IEnumerable<Book> GenerateBooks()
    {
        var books = new List<Book>
        {
            new Book
            {
                Title = "The Great Adventure",
                ISBN = "1234567890123",
                Price = 19.99M,
                Pages = 320,
                BookCover = BookCover.Softcover,
                Used = false,
                ImageUrl = "",
                Description = "An exciting journey through uncharted lands.",
                AuthorId = Guid.Parse("A5E87971-53AD-40DF-97FF-79DCAEF4520A"),
                CategoryId = 1
            },
            new Book
            {
                Title = "Science and You",
                ISBN = "9876543210987",
                Price = 25.99M,
                Pages = 220,
                BookCover = BookCover.Hardcover,
                Used = true,
                ImageUrl = "",
                Description = "Exploring the wonders of science in everyday life.",
                AuthorId = Guid.Parse("72FC4A67-9B6D-44E0-A21A-CC78BA323DEA"),
                CategoryId = 2
            }
        };

        return books;
    }
}