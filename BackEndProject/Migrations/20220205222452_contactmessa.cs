using Microsoft.EntityFrameworkCore.Migrations;

namespace BackEndProject.Migrations
{
    public partial class contactmessa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContactMessage_AspNetUsers_AppUserId",
                table: "ContactMessage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ContactMessage",
                table: "ContactMessage");

            migrationBuilder.RenameTable(
                name: "ContactMessage",
                newName: "ContactMessages");

            migrationBuilder.RenameIndex(
                name: "IX_ContactMessage_AppUserId",
                table: "ContactMessages",
                newName: "IX_ContactMessages_AppUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContactMessages",
                table: "ContactMessages",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ContactMessages_AspNetUsers_AppUserId",
                table: "ContactMessages",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContactMessages_AspNetUsers_AppUserId",
                table: "ContactMessages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ContactMessages",
                table: "ContactMessages");

            migrationBuilder.RenameTable(
                name: "ContactMessages",
                newName: "ContactMessage");

            migrationBuilder.RenameIndex(
                name: "IX_ContactMessages_AppUserId",
                table: "ContactMessage",
                newName: "IX_ContactMessage_AppUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContactMessage",
                table: "ContactMessage",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ContactMessage_AspNetUsers_AppUserId",
                table: "ContactMessage",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
