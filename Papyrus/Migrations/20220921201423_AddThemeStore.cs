using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Papyrus.Migrations
{
    public partial class AddThemeStore : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Theme",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "Translations",
                columns: new[] { "Key", "Language", "Segment", "Value" },
                values: new object[,]
                {
                    { "0", "en-US", "Theme", "Light Theme" },
                    { "0", "hu-HU", "Theme", "Világos Téma" },
                    { "1", "en-US", "Theme", "Dark Theme" },
                    { "1", "hu-HU", "Theme", "Sötét Téma" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "0", "en-US", "Theme" });

            migrationBuilder.DeleteData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "0", "hu-HU", "Theme" });

            migrationBuilder.DeleteData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "1", "en-US", "Theme" });

            migrationBuilder.DeleteData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "1", "hu-HU", "Theme" });

            migrationBuilder.DropColumn(
                name: "Theme",
                table: "AspNetUsers");
        }
    }
}
