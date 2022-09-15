using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Papyrus.Migrations
{
    public partial class NoteArchiveStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Archived",
                table: "Notes",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "Translations",
                columns: new[] { "Key", "Language", "Segment", "Value" },
                values: new object[] { "Archived", "en-US", "Note", "Archived status is changed" });

            migrationBuilder.InsertData(
                table: "Translations",
                columns: new[] { "Key", "Language", "Segment", "Value" },
                values: new object[] { "Archived", "hu-HU", "Note", "Archivált státusz megváltoztatva" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "Archived", "en-US", "Note" });

            migrationBuilder.DeleteData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "Archived", "hu-HU", "Note" });

            migrationBuilder.DropColumn(
                name: "Archived",
                table: "Notes");
        }
    }
}
