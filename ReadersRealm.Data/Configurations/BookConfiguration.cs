namespace ReadersRealm.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;
using Models.Enums;

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
                Title = "Naruto Vol 1",
                ISBN = "9797415363864",
                Price = 19.99M,
                Pages = 192,
                BookCover = BookCover.Softcover,
                Used = false,
                ImageUrl = "images\\bookSeedData\\9e84aff9-028a-4cd7-8e31-2e73eddce1d5.jpg",
                Description = @"""Naruto Vol 1"" introduces readers to Naruto Uzumaki, a young ninja with dreams of becoming the strongest ninja and leader of his village, the Hokage. Despite being ostracized by the village for housing a fearsome nine-tailed fox that attacked the village years ago, Naruto is determined to gain the village's recognition through his accomplishments and hard work. The volume covers his struggles in the Ninja Academy, his formation of friendships and rivalries, and the beginning of his journey to becoming a skilled ninja. It sets the stage for an expansive story of growth, battles, and the pursuit of dreams amidst the complexities of the ninja world.",
                AuthorId = Guid.Parse("A5E87971-53AD-40DF-97FF-79DCAEF4520A"),
                CategoryId = 1,
            },
            new Book
            {
                Title = "Naruto Vol 8",
                ISBN = "9785421961182",
                Price = 19.99M,
                Pages = 192,
                BookCover = BookCover.Softcover,
                Used = false,
                ImageUrl = "images\\bookSeedData\\b2f6e888-402f-4851-9392-8931a567ea9a.jpg",
                Description = @"In ""Naruto Vol 8"" the Chunin Exam's second phase plunges Naruto, Sasuke, and Sakura into the perilous Forest of Death. Amidst treacherous battles and survival challenges, the team confronts formidable rivals and their own fears. As hidden dangers emerge, Naruto's resolve is tested, unveiling new powers and deepening bonds. This volume is a thrilling journey of growth, teamwork, and unyielding spirit against life-and-death stakes.",
                AuthorId = Guid.Parse("A5E87971-53AD-40DF-97FF-79DCAEF4520A"),
                CategoryId = 1,
            },
            new Book
            {
                Title = "Naruto Vol 35",
                ISBN = "9799667150078",
                Price = 19.99M,
                Pages = 200,
                BookCover = BookCover.Softcover,
                Used = false,
                ImageUrl = "images\\bookSeedData\\3ba49236-d6a4-43ec-b9d7-d77eeff8e812.webp",
                Description = @"""Naruto Vol 35"" shifts the focus to the aftermath of Naruto's intense training under Jiraiya and the looming threat of the Akatsuki. Naruto returns to Konoha, showcasing newfound strengths and techniques. Meanwhile, the story delves into the formation of new alliances and the preparation for a confrontation that could shake the ninja world to its core. This volume encapsulates themes of evolution and anticipation, setting the stage for epic battles and strategic mind games.",
                AuthorId = Guid.Parse("A5E87971-53AD-40DF-97FF-79DCAEF4520A"),
                CategoryId = 1,
            },
        };

        return books;
    }
}