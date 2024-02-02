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
                columns: new[] { "Id", "AuthorId", "BookCover", "CategoryId", "Description", "ISBN", "Pages", "Price", "Title", "Used" },
                values: new object[,]
                {
                    { new Guid("17358943-a431-4fcf-810f-b02d16a2da83"), new Guid("72fc4a67-9b6d-44e0-a21a-cc78ba323dea"), 0, 2, "Exploring the wonders of science in everyday life.", "9876543210987", 220, 25.99m, "Science and You", true },
                    { new Guid("e74704c4-7d6d-48ff-a101-ef414bc756f8"), new Guid("a5e87971-53ad-40df-97ff-79dcaef4520a"), 1, 1, "An exciting journey through uncharted lands.", "1234567890123", 320, 19.99m, "The Great Adventure", false }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("17358943-a431-4fcf-810f-b02d16a2da83"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("e74704c4-7d6d-48ff-a101-ef414bc756f8"));
        }
    }
}
