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
            new Author
            {
                Id = new Guid("2d340d56-2594-4071-8878-019647b45cb4"),
                FirstName = "Gege",
                LastName = "Akutami",
                Email = "gegeakutami@gmail.com",
                PhoneNumber = "098-354-4364",
                Gender = Gender.Male,
                Age = 32,
            },
            new Author
            {
                Id = new Guid("4f82ee5a-9a3b-4d00-8027-e677e41e2527"),
                FirstName = "Kentaro",
                LastName = "Miura",
                Email = "kentaromiura@gmail.com",
                PhoneNumber = "098-646-4683",
                Gender = Gender.Male,
                Age = 54,
            },
            new Author
            {
                Id = new Guid("99aa315d-72fb-49fe-a18b-fc2b7c84ec51"),
                FirstName = "Eiichiro",
                LastName = "Oda",
                Email = "eiichirooda@gmail.com",
                PhoneNumber = "098-742-7674",
                Gender = Gender.Male,
                Age = 49,
            },
        };

        return authors;
    }
}