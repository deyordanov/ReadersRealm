using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ReadersRealm.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedCategoryAndAuthorTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "Id", "Age", "Email", "FirstName", "Gender", "LastName", "MiddleName", "PhoneNumber" },
                values: new object[,]
                {
                    { new Guid("72fc4a67-9b6d-44e0-a21a-cc78ba323dea"), 59, "rriordan@gmail.com", "Rick", 0, "Riordan", null, "098-765-4321" },
                    { new Guid("a5e87971-53ad-40df-97ff-79dcaef4520a"), 49, "kmasashi@gmail.com", "Masashi", 0, "Kishimoto", null, "123-456-7890" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "DisplayOrder", "Name" },
                values: new object[,]
                {
                    { 1, 1, "Manga" },
                    { 2, 2, "Action" },
                    { 3, 3, "SciFi" },
                    { 4, 4, "History" },
                    { 5, 5, "Fantasy" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: new Guid("72fc4a67-9b6d-44e0-a21a-cc78ba323dea"));

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: new Guid("a5e87971-53ad-40df-97ff-79dcaef4520a"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
