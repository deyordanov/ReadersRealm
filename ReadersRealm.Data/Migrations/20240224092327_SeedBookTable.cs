using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ReadersRealm.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedBookTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "AuthorId", "BookCover", "CategoryId", "Description", "ISBN", "ImageUrl", "Pages", "Price", "Title", "Used" },
                values: new object[,]
                {
                    { new Guid("09208dff-2d0d-48c8-9375-a7218f652b07"), new Guid("a5e87971-53ad-40df-97ff-79dcaef4520a"), 1, 1, "\"Naruto Vol 35\" shifts the focus to the aftermath of Naruto's intense training under Jiraiya and the looming threat of the Akatsuki. Naruto returns to Konoha, showcasing newfound strengths and techniques. Meanwhile, the story delves into the formation of new alliances and the preparation for a confrontation that could shake the ninja world to its core. This volume encapsulates themes of evolution and anticipation, setting the stage for epic battles and strategic mind games.", "9799667150078", "images\\bookSeedData\\3ba49236-d6a4-43ec-b9d7-d77eeff8e812.webp", 200, 19.99m, "Naruto Vol 35", false },
                    { new Guid("56c5a7f4-0eb0-4a79-8aeb-78fdb9c3a581"), new Guid("a5e87971-53ad-40df-97ff-79dcaef4520a"), 1, 1, "In \"Naruto Vol 8\" the Chunin Exam's second phase plunges Naruto, Sasuke, and Sakura into the perilous Forest of Death. Amidst treacherous battles and survival challenges, the team confronts formidable rivals and their own fears. As hidden dangers emerge, Naruto's resolve is tested, unveiling new powers and deepening bonds. This volume is a thrilling journey of growth, teamwork, and unyielding spirit against life-and-death stakes.", "9785421961182", "images\\bookSeedData\\b2f6e888-402f-4851-9392-8931a567ea9a.jpg", 192, 19.99m, "Naruto Vol 8", false },
                    { new Guid("9a221b67-2480-4140-a40b-ff7ad030c38b"), new Guid("a5e87971-53ad-40df-97ff-79dcaef4520a"), 1, 1, "\"Naruto Vol 1\" introduces readers to Naruto Uzumaki, a young ninja with dreams of becoming the strongest ninja and leader of his village, the Hokage. Despite being ostracized by the village for housing a fearsome nine-tailed fox that attacked the village years ago, Naruto is determined to gain the village's recognition through his accomplishments and hard work. The volume covers his struggles in the Ninja Academy, his formation of friendships and rivalries, and the beginning of his journey to becoming a skilled ninja. It sets the stage for an expansive story of growth, battles, and the pursuit of dreams amidst the complexities of the ninja world.", "9797415363864", "images\\bookSeedData\\9e84aff9-028a-4cd7-8e31-2e73eddce1d5.jpg", 192, 19.99m, "Naruto Vol 1", false }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("09208dff-2d0d-48c8-9375-a7218f652b07"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("56c5a7f4-0eb0-4a79-8aeb-78fdb9c3a581"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("9a221b67-2480-4140-a40b-ff7ad030c38b"));
        }
    }
}
