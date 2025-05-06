//using Microsoft.EntityFrameworkCore.Migrations;

//#nullable disable

//namespace LibraryDashboard2.Migrations
//{
//    /// <inheritdoc />
//    public partial class UpdateBookModel : Migration
//    {
//        /// <inheritdoc />
//        protected override void Up(MigrationBuilder migrationBuilder)
//        {
//            migrationBuilder.RenameColumn(
//                name: "Title",
//                table: "Books",
//                newName: "Genre");

//            migrationBuilder.AddColumn<int>(
//                name: "BookId",
//                table: "Books",
//                type: "int",
//                nullable: false,
//                defaultValue: 0);

//            migrationBuilder.AddColumn<string>(
//                name: "BookName",
//                table: "Books",
//                type: "nvarchar(max)",
//                nullable: false,
//                defaultValue: "");
//        }

//        /// <inheritdoc />
//        protected override void Down(MigrationBuilder migrationBuilder)
//        {
//            migrationBuilder.DropColumn(
//                name: "BookId",
//                table: "Books");

//            migrationBuilder.DropColumn(
//                name: "BookName",
//                table: "Books");

//            migrationBuilder.RenameColumn(
//                name: "Genre",
//                table: "Books",
//                newName: "Title");
//        }
//    }
//}


using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryDashboard2.Migrations
{
    public partial class UpdateBookModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
            //migrationBuilder.AddColumn<int>(
            //    name: "BookId",
            //    table: "Books",
            //    type: "int",
            //    nullable: false,
            //    defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "BookName",
                table: "Books",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Genre",
                table: "Books",
                type: "nvarchar(max)",
                nullable: true); // genre might be nullable
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookId",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "BookName",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "Genre",
                table: "Books");
        }
    }
}
