namespace ReadersRealm.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;
using ReadersRealm.Data.Models.Enums;

public class AuthorConfiguration : IEntityTypeConfiguration<Author>
{
    public void Configure(EntityTypeBuilder<Author> builder)
    {
        builder.HasData(GenerateAuthors());
    }

    public static IEnumerable<Author> GenerateAuthors()
    {
        var authors = new List<Author>
        {
            new Author
            {
                Id = new Guid("A5E87971-53AD-40DF-97FF-79DCAEF4520A"),
                FirstName = "John",
                LastName = "Smith",
                Email = "johnsmith@example.com",
                PhoneNumber = "123-456-7890",
                Gender = Gender.Male,
                Age = 45,
            },
            new Author
            {
                Id = new Guid("72FC4A67-9B6D-44E0-A21A-CC78BA323DEA"),
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
}