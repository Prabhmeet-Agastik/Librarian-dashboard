using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryDashboard2.Migrations
{
    /// <inheritdoc />
    public partial class AddBook4Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BookName",
                table: "Books",
                newName: "Title");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Books",
                newName: "BookName");
        }
    }
}
