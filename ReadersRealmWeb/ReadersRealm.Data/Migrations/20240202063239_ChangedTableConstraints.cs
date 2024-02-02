using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ReadersRealm.Migrations
{
    /// <inheritdoc />
    public partial class ChangedTableConstraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("af609722-7e21-4c39-9780-7bb0dcaac117"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("d17d806c-8a12-443f-861c-8b650f3bd058"));

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: new Guid("2c179598-dab8-444f-ab43-053752577ed7"));

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: new Guid("f92e09e6-9709-44e8-abe0-d203801a25af"));

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Books",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                comment: "Book Description",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldComment: "Book Description");

            migrationBuilder.AlterColumn<string>(
                name: "MiddleName",
                table: "Authors",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: true,
                comment: "Author Middle Name",
                oldClrType: typeof(string),
                oldType: "nvarchar(40)",
                oldMaxLength: 40,
                oldComment: "Author Middle Name");

            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "Id", "Age", "Email", "FirstName", "Gender", "LastName", "MiddleName", "PhoneNumber" },
                values: new object[,]
                {
                    { new Guid("72fc4a67-9b6d-44e0-a21a-cc78ba323dea"), 45, "johnsmith@example.com", "John", 0, "Smith", null, "123-456-7890" },
                    { new Guid("a5e87971-53ad-40df-97ff-79dcaef4520a"), 38, "emilyjohnson@example.com", "Emily", 1, "Johnson", null, "098-765-4321" }
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "AuthorId", "BookCover", "Description", "ISBN", "Pages", "Price", "Title", "Used" },
                values: new object[,]
                {
                    { new Guid("2eedac4e-da96-4138-b455-94352227de62"), new Guid("a5e87971-53ad-40df-97ff-79dcaef4520a"), 0, "Exploring the wonders of science in everyday life.", "9876543210987", 220, 25.99m, "Science and You", true },
                    { new Guid("426998a2-eaca-42e5-ae61-ad3adacd3e97"), new Guid("72fc4a67-9b6d-44e0-a21a-cc78ba323dea"), 1, "An exciting journey through uncharted lands.", "1234567890123", 320, 19.99m, "The Great Adventure", false }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("2eedac4e-da96-4138-b455-94352227de62"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("426998a2-eaca-42e5-ae61-ad3adacd3e97"));

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: new Guid("72fc4a67-9b6d-44e0-a21a-cc78ba323dea"));

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: new Guid("a5e87971-53ad-40df-97ff-79dcaef4520a"));

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Books",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                comment: "Book Description",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true,
                oldComment: "Book Description");

            migrationBuilder.AlterColumn<string>(
                name: "MiddleName",
                table: "Authors",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "",
                comment: "Author Middle Name",
                oldClrType: typeof(string),
                oldType: "nvarchar(40)",
                oldMaxLength: 40,
                oldNullable: true,
                oldComment: "Author Middle Name");

            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "Id", "Age", "Email", "FirstName", "Gender", "LastName", "MiddleName", "PhoneNumber" },
                values: new object[,]
                {
                    { new Guid("2c179598-dab8-444f-ab43-053752577ed7"), 38, "emilyjohnson@example.com", "Emily", 1, "Johnson", "", "098-765-4321" },
                    { new Guid("f92e09e6-9709-44e8-abe0-d203801a25af"), 45, "johnsmith@example.com", "John", 0, "Smith", "", "123-456-7890" }
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "AuthorId", "BookCover", "Description", "ISBN", "Pages", "Price", "Title", "Used" },
                values: new object[,]
                {
                    { new Guid("af609722-7e21-4c39-9780-7bb0dcaac117"), new Guid("f92e09e6-9709-44e8-abe0-d203801a25af"), 1, "An exciting journey through uncharted lands.", "1234567890123", 320, 19.99m, "The Great Adventure", false },
                    { new Guid("d17d806c-8a12-443f-861c-8b650f3bd058"), new Guid("2c179598-dab8-444f-ab43-053752577ed7"), 0, "Exploring the wonders of science in everyday life.", "9876543210987", 220, 25.99m, "Science and You", true }
                });
        }
    }
}
