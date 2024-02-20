namespace ReadersRealm.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;
using Models.Enums;

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
                FirstName = "Masashi",
                LastName = "Kishimoto",
                Email = "kmasashi@gmail.com",
                PhoneNumber = "123-456-7890",
                Gender = Gender.Male,
                Age = 49,
            },
            new Author
            {
                Id = new Guid("72FC4A67-9B6D-44E0-A21A-CC78BA323DEA"),
                FirstName = "Rick",
                LastName = "Riordan",
                Email = "rriordan@gmail.com",
                PhoneNumber = "098-765-4321",
                Gender = Gender.Male,
                Age = 59,
            },
        };

        return authors;
    }
}