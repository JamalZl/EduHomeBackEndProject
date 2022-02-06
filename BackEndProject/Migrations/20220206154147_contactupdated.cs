using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BackEndProject.Migrations
{
    public partial class contactupdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContactMessages_AspNetUsers_AppUserId",
                table: "ContactMessages");

            migrationBuilder.DropIndex(
                name: "IX_ContactMessages_AppUserId",
                table: "ContactMessages");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "ContactMessages");

            migrationBuilder.DropColumn(
                name: "CreatedTime",
                table: "ContactMessages");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "ContactMessages",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "ContactMessages");

            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "ContactMessages",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedTime",
                table: "ContactMessages",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_ContactMessages_AppUserId",
                table: "ContactMessages",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ContactMessages_AspNetUsers_AppUserId",
                table: "ContactMessages",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
