using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Papyrus.Migrations
{
    public partial class NoteContentToStore : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Content",
                table: "Notes",
                newName: "ContentId");

            migrationBuilder.AddColumn<DateTime>(
                name: "ContentLastEdit",
                table: "Notes",
                type: "datetime(6)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContentLastEdit",
                table: "Notes");

            migrationBuilder.RenameColumn(
                name: "ContentId",
                table: "Notes",
                newName: "Content");
        }
    }
}
