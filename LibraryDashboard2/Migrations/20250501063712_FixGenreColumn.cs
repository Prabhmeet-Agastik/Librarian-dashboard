using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryDashboard2.Migrations
{
    /// <inheritdoc />
    public partial class FixGenreColumn : Migration
    {
        /// <inheritdoc />
        /// 


        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Books",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "Books",
                newName: "Quantity");

            migrationBuilder.RenameColumn(
                name: "Genre",
                table: "Books",
                newName: "Genre");

            migrationBuilder.RenameColumn(
                name: "BookId",
                table: "Books",
                newName: "BookId");

            migrationBuilder.RenameColumn(
                name: "Author",
                table: "Books",
                newName: "Author");

        }

        /// <inheritdoc />
        /// 
        protected override void Down(MigrationBuilder migrationBuilder)
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
    }
}
