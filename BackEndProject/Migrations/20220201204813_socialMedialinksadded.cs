using Microsoft.EntityFrameworkCore.Migrations;

namespace BackEndProject.Migrations
{
    public partial class socialMedialinksadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FacebookUrl",
                table: "Teachers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PinterestUrl",
                table: "Teachers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TwitterUrl",
                table: "Teachers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VimeoUrl",
                table: "Teachers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FacebookUrl",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "PinterestUrl",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "TwitterUrl",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "VimeoUrl",
                table: "Teachers");
        }
    }
}
