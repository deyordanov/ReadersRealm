namespace ReadersRealm.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasData(GenerateCategories());
    }

    private static IEnumerable<Category> GenerateCategories()
    {
        return new HashSet<Category>()
        {
            new Category
            {
                Id = 1,
                Name = "Manga",
                DisplayOrder = 1,
            },
            new Category()
            {
                Id = 2,
                Name = "Action",
                DisplayOrder = 2,
            },
            new Category()
            {
                Id = 3,
                Name = "SciFi",
                DisplayOrder = 3,
            },
            new Category()
            {
                Id = 4,
                Name = "History",
                DisplayOrder = 4,
            },
            new Category()
            {
                Id = 5,
                Name = "Fantasy",
                DisplayOrder = 5,
            },
        };
    }
}