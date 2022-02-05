using Microsoft.EntityFrameworkCore.Migrations;

namespace BackEndProject.Migrations
{
    public partial class contactmessage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContactMessage_AspNetUsers_AppUserId1",
                table: "ContactMessage");

            migrationBuilder.DropIndex(
                name: "IX_ContactMessage_AppUserId1",
                table: "ContactMessage");

            migrationBuilder.DropColumn(
                name: "AppUserId1",
                table: "ContactMessage");

            migrationBuilder.AlterColumn<string>(
                name: "AppUserId",
                table: "ContactMessage",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_ContactMessage_AppUserId",
                table: "ContactMessage",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ContactMessage_AspNetUsers_AppUserId",
                table: "ContactMessage",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContactMessage_AspNetUsers_AppUserId",
                table: "ContactMessage");

            migrationBuilder.DropIndex(
                name: "IX_ContactMessage_AppUserId",
                table: "ContactMessage");

            migrationBuilder.AlterColumn<int>(
                name: "AppUserId",
                table: "ContactMessage",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AppUserId1",
                table: "ContactMessage",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ContactMessage_AppUserId1",
                table: "ContactMessage",
                column: "AppUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ContactMessage_AspNetUsers_AppUserId1",
                table: "ContactMessage",
                column: "AppUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
