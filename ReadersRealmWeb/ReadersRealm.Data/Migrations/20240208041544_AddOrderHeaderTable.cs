using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ReadersRealm.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderHeaderTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("678d0764-ae5d-48cb-9c46-b52c3a691ee8"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("74ba11d7-4c0e-4f9d-9912-7dcf8aad02f7"));

            migrationBuilder.CreateTable(
                name: "OrderHeaders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ShippingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OrderTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OrderStatus = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    PaymentStatus = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    TrackingNumber = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Carrier = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentDueDate = table.Column<DateOnly>(type: "date", nullable: false),
                    PaymentIntentId = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderHeaders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderHeaders_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "AuthorId", "BookCover", "CategoryId", "Description", "ISBN", "ImageUrl", "Pages", "Price", "Title", "Used" },
                values: new object[,]
                {
                    { new Guid("bcb742c3-c769-4bf8-80b8-8df29c173890"), new Guid("72fc4a67-9b6d-44e0-a21a-cc78ba323dea"), 0, 2, "Exploring the wonders of science in everyday life.", "9876543210987", "", 220, 25.99m, "Science and You", true },
                    { new Guid("ed0c6e16-51d0-4dec-bc0e-91f0b9c48337"), new Guid("a5e87971-53ad-40df-97ff-79dcaef4520a"), 1, 1, "An exciting journey through uncharted lands.", "1234567890123", "", 320, 19.99m, "The Great Adventure", false }
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderHeaders_ApplicationUserId",
                table: "OrderHeaders",
                column: "ApplicationUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderHeaders");

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("bcb742c3-c769-4bf8-80b8-8df29c173890"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("ed0c6e16-51d0-4dec-bc0e-91f0b9c48337"));

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "AuthorId", "BookCover", "CategoryId", "Description", "ISBN", "ImageUrl", "Pages", "Price", "Title", "Used" },
                values: new object[,]
                {
                    { new Guid("678d0764-ae5d-48cb-9c46-b52c3a691ee8"), new Guid("72fc4a67-9b6d-44e0-a21a-cc78ba323dea"), 0, 2, "Exploring the wonders of science in everyday life.", "9876543210987", "", 220, 25.99m, "Science and You", true },
                    { new Guid("74ba11d7-4c0e-4f9d-9912-7dcf8aad02f7"), new Guid("a5e87971-53ad-40df-97ff-79dcaef4520a"), 1, 1, "An exciting journey through uncharted lands.", "1234567890123", "", 320, 19.99m, "The Great Adventure", false }
                });
        }
    }
}
