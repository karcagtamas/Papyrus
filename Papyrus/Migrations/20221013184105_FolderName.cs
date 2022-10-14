using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Papyrus.Migrations
{
    public partial class FolderName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Folders",
                newName: "Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Folders",
                newName: "Title");
        }
    }
}
