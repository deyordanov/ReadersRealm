using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReadersRealm.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Authors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Author Identifier"),
                    FirstName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false, comment: "Author First Name"),
                    MiddleName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true, comment: "Author Middle Name"),
                    LastName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false, comment: "Author Last Name"),
                    Age = table.Column<int>(type: "int", nullable: true, comment: "Author Age"),
                    Gender = table.Column<int>(type: "int", nullable: false, comment: "Author Gender"),
                    Email = table.Column<string>(type: "nvarchar(254)", maxLength: 254, nullable: false, comment: "Author Email"),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "Author Phone Number")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.Id);
                },
                comment: "Readers Realm Author");

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Category Identifier")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "Category Name"),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false, comment: "Category Display Order")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                },
                comment: "Readers Realm Category");

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Book Identifier"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Book Title"),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "Book Description"),
                    ISBN = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false, comment: "Book's International Standard Book Number"),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false, comment: "Book Price"),
                    Pages = table.Column<int>(type: "int", nullable: true, comment: "Book Page Count"),
                    BookCover = table.Column<int>(type: "int", nullable: true, comment: "Book Cover Type"),
                    Used = table.Column<bool>(type: "bit", nullable: false, comment: "Book Condition"),
                    AuthorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Author Identifier"),
                    CategoryId = table.Column<int>(type: "int", nullable: false, comment: "Category Identifier")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Books_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Books_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Readers Realm Book");

            migrationBuilder.CreateIndex(
                name: "IX_Books_AuthorId",
                table: "Books",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Books_CategoryId",
                table: "Books",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "Authors");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
