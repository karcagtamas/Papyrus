using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Papyrus.Migrations
{
    public partial class GroupEdit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EditTag",
                table: "GroupRoles",
                newName: "GroupEdit");

            migrationBuilder.RenameColumn(
                name: "EditRole",
                table: "GroupRoles",
                newName: "GroupClose");

            migrationBuilder.RenameColumn(
                name: "EditMember",
                table: "GroupRoles",
                newName: "EditTagList");

            migrationBuilder.RenameColumn(
                name: "Edit",
                table: "GroupRoles",
                newName: "EditRoleList");

            migrationBuilder.RenameColumn(
                name: "Close",
                table: "GroupRoles",
                newName: "EditMemberList");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "GroupEdit",
                table: "GroupRoles",
                newName: "EditTag");

            migrationBuilder.RenameColumn(
                name: "GroupClose",
                table: "GroupRoles",
                newName: "EditRole");

            migrationBuilder.RenameColumn(
                name: "EditTagList",
                table: "GroupRoles",
                newName: "EditMember");

            migrationBuilder.RenameColumn(
                name: "EditRoleList",
                table: "GroupRoles",
                newName: "Edit");

            migrationBuilder.RenameColumn(
                name: "EditMemberList",
                table: "GroupRoles",
                newName: "Close");
        }
    }
}
