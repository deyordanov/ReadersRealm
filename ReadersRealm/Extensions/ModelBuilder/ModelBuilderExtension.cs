namespace ReadersRealm.Extensions.ModelBuilder;

using Microsoft.EntityFrameworkCore;
using ReadersRealm.Data.Models;

public static class ModelBuilderExtension
{
    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>().HasData(GenerateCategories());
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
}