using Microsoft.EntityFrameworkCore.Migrations;

namespace BackEndProject.Migrations
{
    public partial class createdsettingsandfootersocials : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SiteLogo = table.Column<string>(maxLength: 50, nullable: true),
                    HeaderNumber = table.Column<string>(maxLength: 50, nullable: true),
                    SearchIcon = table.Column<string>(maxLength: 50, nullable: true),
                    FooterDescription = table.Column<string>(maxLength: 500, nullable: true),
                    Address = table.Column<string>(maxLength: 50, nullable: true),
                    FooterNumber1 = table.Column<string>(maxLength: 50, nullable: true),
                    FooterNumber2 = table.Column<string>(maxLength: 50, nullable: true),
                    FooterMail = table.Column<string>(maxLength: 50, nullable: true),
                    FooterWebsite = table.Column<string>(maxLength: 50, nullable: true),
                    FooterHasTechUrl = table.Column<string>(maxLength: 200, nullable: true),
                    AboutTitle = table.Column<string>(maxLength: 100, nullable: true),
                    AboutDescription = table.Column<string>(maxLength: 500, nullable: true),
                    AboutImage = table.Column<string>(maxLength: 80, nullable: true),
                    AboutViewUrl = table.Column<string>(maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FooterSocials",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Logo = table.Column<string>(maxLength: 70, nullable: false),
                    LogoUrl = table.Column<string>(maxLength: 200, nullable: false),
                    SettingId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FooterSocials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FooterSocials_Settings_SettingId",
                        column: x => x.SettingId,
                        principalTable: "Settings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FooterSocials_SettingId",
                table: "FooterSocials",
                column: "SettingId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FooterSocials");

            migrationBuilder.DropTable(
                name: "Settings");
        }
    }
}
