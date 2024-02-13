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
}