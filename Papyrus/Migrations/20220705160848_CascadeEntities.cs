using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Papyrus.Migrations
{
    public partial class CascadeEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupActionLogs_Groups_GroupId",
                table: "GroupActionLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupMembers_AspNetUsers_AddedById",
                table: "GroupMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupMembers_AspNetUsers_UserId",
                table: "GroupMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupMembers_GroupRoles_RoleId",
                table: "GroupMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupMembers_Groups_GroupId",
                table: "GroupMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupRoles_Groups_GroupId",
                table: "GroupRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_Groups_AspNetUsers_OwnerId",
                table: "Groups");

            migrationBuilder.DropForeignKey(
                name: "FK_Notes_AspNetUsers_UserId",
                table: "Notes");

            migrationBuilder.DropForeignKey(
                name: "FK_Notes_Groups_GroupId",
                table: "Notes");

            migrationBuilder.DropForeignKey(
                name: "FK_Tags_AspNetUsers_UserId",
                table: "Tags");

            migrationBuilder.DropForeignKey(
                name: "FK_Tags_Groups_GroupId",
                table: "Tags");

            migrationBuilder.DropForeignKey(
                name: "FK_Tags_Tags_ParentId",
                table: "Tags");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupActionLogs_Groups_GroupId",
                table: "GroupActionLogs",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupMembers_AspNetUsers_AddedById",
                table: "GroupMembers",
                column: "AddedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupMembers_AspNetUsers_UserId",
                table: "GroupMembers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupMembers_GroupRoles_RoleId",
                table: "GroupMembers",
                column: "RoleId",
                principalTable: "GroupRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupMembers_Groups_GroupId",
                table: "GroupMembers",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupRoles_Groups_GroupId",
                table: "GroupRoles",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_AspNetUsers_OwnerId",
                table: "Groups",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_AspNetUsers_UserId",
                table: "Notes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_Groups_GroupId",
                table: "Notes",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_AspNetUsers_UserId",
                table: "Tags",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_Groups_GroupId",
                table: "Tags",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_Tags_ParentId",
                table: "Tags",
                column: "ParentId",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupActionLogs_Groups_GroupId",
                table: "GroupActionLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupMembers_AspNetUsers_AddedById",
                table: "GroupMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupMembers_AspNetUsers_UserId",
                table: "GroupMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupMembers_GroupRoles_RoleId",
                table: "GroupMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupMembers_Groups_GroupId",
                table: "GroupMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupRoles_Groups_GroupId",
                table: "GroupRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_Groups_AspNetUsers_OwnerId",
                table: "Groups");

            migrationBuilder.DropForeignKey(
                name: "FK_Notes_AspNetUsers_UserId",
                table: "Notes");

            migrationBuilder.DropForeignKey(
                name: "FK_Notes_Groups_GroupId",
                table: "Notes");

            migrationBuilder.DropForeignKey(
                name: "FK_Tags_AspNetUsers_UserId",
                table: "Tags");

            migrationBuilder.DropForeignKey(
                name: "FK_Tags_Groups_GroupId",
                table: "Tags");

            migrationBuilder.DropForeignKey(
                name: "FK_Tags_Tags_ParentId",
                table: "Tags");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupActionLogs_Groups_GroupId",
                table: "GroupActionLogs",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupMembers_AspNetUsers_AddedById",
                table: "GroupMembers",
                column: "AddedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupMembers_AspNetUsers_UserId",
                table: "GroupMembers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupMembers_GroupRoles_RoleId",
                table: "GroupMembers",
                column: "RoleId",
                principalTable: "GroupRoles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupMembers_Groups_GroupId",
                table: "GroupMembers",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupRoles_Groups_GroupId",
                table: "GroupRoles",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_AspNetUsers_OwnerId",
                table: "Groups",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_AspNetUsers_UserId",
                table: "Notes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_Groups_GroupId",
                table: "Notes",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_AspNetUsers_UserId",
                table: "Tags",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_Groups_GroupId",
                table: "Tags",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_Tags_ParentId",
                table: "Tags",
                column: "ParentId",
                principalTable: "Tags",
                principalColumn: "Id");
        }
    }
}
