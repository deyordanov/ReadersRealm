using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ReadersRealm.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("6dd4a7d7-1aff-4c89-9972-5a2258f196a3"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("b82b60c1-f550-4a93-a66f-b9b613061909"));

            migrationBuilder.AddColumn<Guid>(
                name: "OrderId",
                table: "OrdersDetails",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderHeaderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_OrderHeaders_OrderHeaderId",
                        column: x => x.OrderHeaderId,
                        principalTable: "OrderHeaders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "AuthorId", "BookCover", "CategoryId", "Description", "ISBN", "ImageUrl", "Pages", "Price", "Title", "Used" },
                values: new object[,]
                {
                    { new Guid("4dba24af-138e-47af-bf7a-15eef1fe4f49"), new Guid("a5e87971-53ad-40df-97ff-79dcaef4520a"), 1, 1, "An exciting journey through uncharted lands.", "1234567890123", "", 320, 19.99m, "The Great Adventure", false },
                    { new Guid("c315bff6-1cd0-4060-bd16-265dfaed78ab"), new Guid("72fc4a67-9b6d-44e0-a21a-cc78ba323dea"), 0, 2, "Exploring the wonders of science in everyday life.", "9876543210987", "", 220, 25.99m, "Science and You", true }
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrdersDetails_OrderId",
                table: "OrdersDetails",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderHeaderId",
                table: "Orders",
                column: "OrderHeaderId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OrdersDetails_Orders_OrderId",
                table: "OrdersDetails",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrdersDetails_Orders_OrderId",
                table: "OrdersDetails");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_OrdersDetails_OrderId",
                table: "OrdersDetails");

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("4dba24af-138e-47af-bf7a-15eef1fe4f49"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("c315bff6-1cd0-4060-bd16-265dfaed78ab"));

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "OrdersDetails");

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "AuthorId", "BookCover", "CategoryId", "Description", "ISBN", "ImageUrl", "Pages", "Price", "Title", "Used" },
                values: new object[,]
                {
                    { new Guid("6dd4a7d7-1aff-4c89-9972-5a2258f196a3"), new Guid("72fc4a67-9b6d-44e0-a21a-cc78ba323dea"), 0, 2, "Exploring the wonders of science in everyday life.", "9876543210987", "", 220, 25.99m, "Science and You", true },
                    { new Guid("b82b60c1-f550-4a93-a66f-b9b613061909"), new Guid("a5e87971-53ad-40df-97ff-79dcaef4520a"), 1, 1, "An exciting journey through uncharted lands.", "1234567890123", "", 320, 19.99m, "The Great Adventure", false }
                });
        }
    }
}
