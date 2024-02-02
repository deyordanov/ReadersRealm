using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ReadersRealm.Migrations
{
    /// <inheritdoc />
    public partial class AddedEntityConfigurationFiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Books_BookId",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Categories_BookId",
                table: "Categories");

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("2eedac4e-da96-4138-b455-94352227de62"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("426998a2-eaca-42e5-ae61-ad3adacd3e97"));

            migrationBuilder.DropColumn(
                name: "BookId",
                table: "Categories");

            migrationBuilder.AlterColumn<string>(
                name: "ISBN",
                table: "Books",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: false,
                comment: "Book's International Standard Book Number",
                oldClrType: typeof(string),
                oldType: "nvarchar(13)",
                oldMaxLength: 13,
                oldComment: "Book ISBN");

            migrationBuilder.CreateTable(
                name: "BookCategory",
                columns: table => new
                {
                    BooksId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoriesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookCategory", x => new { x.BooksId, x.CategoriesId });
                    table.ForeignKey(
                        name: "FK_BookCategory_Books_BooksId",
                        column: x => x.BooksId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookCategory_Categories_CategoriesId",
                        column: x => x.CategoriesId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: new Guid("72fc4a67-9b6d-44e0-a21a-cc78ba323dea"),
                columns: new[] { "Age", "Email", "FirstName", "Gender", "LastName", "PhoneNumber" },
                values: new object[] { 38, "emilyjohnson@example.com", "Emily", 1, "Johnson", "098-765-4321" });

            migrationBuilder.UpdateData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: new Guid("a5e87971-53ad-40df-97ff-79dcaef4520a"),
                columns: new[] { "Age", "Email", "FirstName", "Gender", "LastName", "PhoneNumber" },
                values: new object[] { 45, "johnsmith@example.com", "John", 0, "Smith", "123-456-7890" });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "AuthorId", "BookCover", "Description", "ISBN", "Pages", "Price", "Title", "Used" },
                values: new object[,]
                {
                    { new Guid("2cec3ffc-2206-446b-874d-0bf1551ea88e"), new Guid("72fc4a67-9b6d-44e0-a21a-cc78ba323dea"), 0, "Exploring the wonders of science in everyday life.", "9876543210987", 220, 25.99m, "Science and You", true },
                    { new Guid("f8f6e08b-6876-4a58-ae56-3c5bcac927a7"), new Guid("a5e87971-53ad-40df-97ff-79dcaef4520a"), 1, "An exciting journey through uncharted lands.", "1234567890123", 320, 19.99m, "The Great Adventure", false }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookCategory_CategoriesId",
                table: "BookCategory",
                column: "CategoriesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookCategory");

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("2cec3ffc-2206-446b-874d-0bf1551ea88e"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("f8f6e08b-6876-4a58-ae56-3c5bcac927a7"));

            migrationBuilder.AddColumn<Guid>(
                name: "BookId",
                table: "Categories",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ISBN",
                table: "Books",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: false,
                comment: "Book ISBN",
                oldClrType: typeof(string),
                oldType: "nvarchar(13)",
                oldMaxLength: 13,
                oldComment: "Book's International Standard Book Number");

            migrationBuilder.UpdateData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: new Guid("72fc4a67-9b6d-44e0-a21a-cc78ba323dea"),
                columns: new[] { "Age", "Email", "FirstName", "Gender", "LastName", "PhoneNumber" },
                values: new object[] { 45, "johnsmith@example.com", "John", 0, "Smith", "123-456-7890" });

            migrationBuilder.UpdateData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: new Guid("a5e87971-53ad-40df-97ff-79dcaef4520a"),
                columns: new[] { "Age", "Email", "FirstName", "Gender", "LastName", "PhoneNumber" },
                values: new object[] { 38, "emilyjohnson@example.com", "Emily", 1, "Johnson", "098-765-4321" });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "AuthorId", "BookCover", "Description", "ISBN", "Pages", "Price", "Title", "Used" },
                values: new object[,]
                {
                    { new Guid("2eedac4e-da96-4138-b455-94352227de62"), new Guid("a5e87971-53ad-40df-97ff-79dcaef4520a"), 0, "Exploring the wonders of science in everyday life.", "9876543210987", 220, 25.99m, "Science and You", true },
                    { new Guid("426998a2-eaca-42e5-ae61-ad3adacd3e97"), new Guid("72fc4a67-9b6d-44e0-a21a-cc78ba323dea"), 1, "An exciting journey through uncharted lands.", "1234567890123", 320, 19.99m, "The Great Adventure", false }
                });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "BookId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "BookId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "BookId",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_BookId",
                table: "Categories",
                column: "BookId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Books_BookId",
                table: "Categories",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id");
        }
    }
}
