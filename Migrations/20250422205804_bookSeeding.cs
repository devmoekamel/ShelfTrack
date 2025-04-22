using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookStore.Migrations
{
    /// <inheritdoc />
    public partial class bookSeeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "Author", "Categoryid", "CoverImageURL", "Description", "Genre", "ISBN", "IsDeleted", "IsFree", "PageCount", "Price", "PublishedDate", "Title" },
                values: new object[,]
                {
                    { 1, "F. Scott Fitzgerald", 1, "https://example.com/gatsby.jpg", "A novel set in the Roaring Twenties.", "Novel", "9780743273565", false, false, 180, 9.9900000000000002, new DateTime(1925, 4, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "The Great Gatsby" },
                    { 2, "F. Scott Fitzgerald", 2, "https://example.com/gatsby.jpg", "A novel set in the Roaring Twenties.", "Novel", "9780743273565", false, false, 180, 9.9900000000000002, new DateTime(1925, 4, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "The Great Gatsby" },
                    { 3, "F. Scott Fitzgerald", 3, "https://example.com/gatsby.jpg", "A novel set in the Roaring Twenties.", "Novel", "9780743273565", false, false, 180, 9.9900000000000002, new DateTime(1925, 4, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "The Great Gatsby" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
