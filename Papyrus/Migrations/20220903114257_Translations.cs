using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Papyrus.Migrations
{
    public partial class Translations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Translations",
                columns: table => new
                {
                    Key = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Segment = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Language = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Value = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Translations", x => new { x.Key, x.Segment, x.Language });
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Translations",
                columns: new[] { "Key", "Language", "Segment", "Value" },
                values: new object[,]
                {
                    { "Administrator", "en-US", "GroupRole", "Administrator" },
                    { "Administrator", "hu-HU", "GroupRole", "Adminisztrátor" },
                    { "Administrator", "en-US", "Role", "Administrator" },
                    { "Administrator", "hu-HU", "Role", "Adminisztrátor" },
                    { "Default", "en-US", "GroupRole", "Default" },
                    { "Default", "hu-HU", "GroupRole", "Alapértelmezett" },
                    { "English", "en-US", "Language", "English" },
                    { "English", "hu-HU", "Language", "Angol" },
                    { "Hungarian", "en-US", "Language", "Hungarian" },
                    { "Hungarian", "hu-HU", "Language", "Magyar" },
                    { "Moderator", "en-US", "GroupRole", "Moderator" },
                    { "Moderator", "hu-HU", "GroupRole", "Moderátor" },
                    { "Moderator", "en-US", "Role", "Moderator" },
                    { "Moderator", "hu-HU", "Role", "Moderátor" },
                    { "User", "en-US", "Role", "User" },
                    { "User", "hu-HU", "Role", "Felhasználó" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Translations");
        }
    }
}
