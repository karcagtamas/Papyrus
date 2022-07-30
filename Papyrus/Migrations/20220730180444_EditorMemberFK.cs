using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Papyrus.Migrations
{
    public partial class EditorMemberFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_EditorMembers_NoteId",
                table: "EditorMembers",
                column: "NoteId");

            migrationBuilder.AddForeignKey(
                name: "FK_EditorMembers_AspNetUsers_UserId",
                table: "EditorMembers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EditorMembers_Notes_NoteId",
                table: "EditorMembers",
                column: "NoteId",
                principalTable: "Notes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EditorMembers_AspNetUsers_UserId",
                table: "EditorMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_EditorMembers_Notes_NoteId",
                table: "EditorMembers");

            migrationBuilder.DropIndex(
                name: "IX_EditorMembers_NoteId",
                table: "EditorMembers");
        }
    }
}
