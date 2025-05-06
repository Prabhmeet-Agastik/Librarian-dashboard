using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryDashboard2.Migrations
{
    /// <inheritdoc />
    public partial class AddBook5Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Books",
                newName: "title");

            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "Books",
                newName: "quantity");

            migrationBuilder.RenameColumn(
                name: "Genre",
                table: "Books",
                newName: "genre");

            migrationBuilder.RenameColumn(
                name: "BookId",
                table: "Books",
                newName: "bookId");

            migrationBuilder.RenameColumn(
                name: "Author",
                table: "Books",
                newName: "author");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "title",
                table: "Books",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "quantity",
                table: "Books",
                newName: "Quantity");

            migrationBuilder.RenameColumn(
                name: "genre",
                table: "Books",
                newName: "Genre");

            migrationBuilder.RenameColumn(
                name: "bookId",
                table: "Books",
                newName: "BookId");

            migrationBuilder.RenameColumn(
                name: "author",
                table: "Books",
                newName: "Author");
        }
    }
}
