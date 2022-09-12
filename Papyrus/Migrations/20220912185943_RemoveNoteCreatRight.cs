using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Papyrus.Migrations
{
    public partial class RemoveNoteCreatRight : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateNote",
                table: "GroupRoles");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CreateNote",
                table: "GroupRoles",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }
    }
}
