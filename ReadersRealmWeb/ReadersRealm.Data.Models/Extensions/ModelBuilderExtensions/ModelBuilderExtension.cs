namespace ReadersRealm.Common.Extensions.ModelBuilderExtensions;

using Data.Models;
using Microsoft.EntityFrameworkCore;
using ReadersRealm.Data.Models.Enums;
using System;

public static class ModelBuilderExtension
{
    public static void Seed(this ModelBuilder modelBuilder)
    {
        IEnumerable<Author> authors = GenerateAuthors();

        modelBuilder.Entity<Category>().HasData(GenerateCategories());
        modelBuilder.Entity<Author>().HasData(authors);
        modelBuilder.Entity<Book>().HasData(GenerateBooks(authors));

    }

    //Seeder for the categories.
    private static IEnumerable<Category> GenerateCategories()
    {
        return new HashSet<Category>()
        {
            new Category()
            {
                Id = 1,
                Name = "Action",
                DisplayOrder = 1,
            },
            new Category()
            {
                Id = 2,
                Name = "SciFi",
                DisplayOrder = 2,
            },
            new Category()
            {
                Id = 3,
                Name = "History",
                DisplayOrder = 3,
            },
        };
    }

    public static IEnumerable<Author> GenerateAuthors()
    {
        var authors = new List<Author>
        {
            new Author
            {
                FirstName = "John",
                LastName = "Smith",
                Email = "johnsmith@example.com",
                PhoneNumber = "123-456-7890",
                Gender = Gender.Male,
                Age = 45,
            },
            new Author
            {
                FirstName = "Emily",
                LastName = "Johnson",
                Email = "emilyjohnson@example.com",
                PhoneNumber = "098-765-4321",
                Gender = Gender.Female,
                Age = 38,
            }
        };

        return authors;
    }

    public static IEnumerable<Book> GenerateBooks(IEnumerable<Author> authors)
    {
        var authorList = new List<Author>(authors);
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
                Description = "An exciting journey through uncharted lands.",
                AuthorId = authorList[0].Id
            },
            new Book
            {
                Title = "Science and You",
                ISBN = "9876543210987",
                Price = 25.99M,
                Pages = 220,
                BookCover = BookCover.Hardcover,
                Used = true,
                Description = "Exploring the wonders of science in everyday life.",
                AuthorId = authorList[1].Id
            }
        };
    
        // authorList[0].Books = new List<Book> { books[0] };
        // authorList[1].Books = new List<Book> { books[1] };
    
        return books;
    }
}