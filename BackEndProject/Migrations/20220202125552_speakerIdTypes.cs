using Microsoft.EntityFrameworkCore.Migrations;

namespace BackEndProject.Migrations
{
    public partial class speakerIdTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Speakers_Companies_CompanyId1",
                table: "Speakers");

            migrationBuilder.DropForeignKey(
                name: "FK_Speakers_Positions_PositionId1",
                table: "Speakers");

            migrationBuilder.DropIndex(
                name: "IX_Speakers_CompanyId1",
                table: "Speakers");

            migrationBuilder.DropIndex(
                name: "IX_Speakers_PositionId1",
                table: "Speakers");

            migrationBuilder.DropColumn(
                name: "CompanyId1",
                table: "Speakers");

            migrationBuilder.DropColumn(
                name: "PositionId1",
                table: "Speakers");

            migrationBuilder.AlterColumn<int>(
                name: "PositionId",
                table: "Speakers",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CompanyId",
                table: "Speakers",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Speakers_CompanyId",
                table: "Speakers",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Speakers_PositionId",
                table: "Speakers",
                column: "PositionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Speakers_Companies_CompanyId",
                table: "Speakers",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Speakers_Positions_PositionId",
                table: "Speakers",
                column: "PositionId",
                principalTable: "Positions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Speakers_Companies_CompanyId",
                table: "Speakers");

            migrationBuilder.DropForeignKey(
                name: "FK_Speakers_Positions_PositionId",
                table: "Speakers");

            migrationBuilder.DropIndex(
                name: "IX_Speakers_CompanyId",
                table: "Speakers");

            migrationBuilder.DropIndex(
                name: "IX_Speakers_PositionId",
                table: "Speakers");

            migrationBuilder.AlterColumn<string>(
                name: "PositionId",
                table: "Speakers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "CompanyId",
                table: "Speakers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "CompanyId1",
                table: "Speakers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PositionId1",
                table: "Speakers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Speakers_CompanyId1",
                table: "Speakers",
                column: "CompanyId1");

            migrationBuilder.CreateIndex(
                name: "IX_Speakers_PositionId1",
                table: "Speakers",
                column: "PositionId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Speakers_Companies_CompanyId1",
                table: "Speakers",
                column: "CompanyId1",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Speakers_Positions_PositionId1",
                table: "Speakers",
                column: "PositionId1",
                principalTable: "Positions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
