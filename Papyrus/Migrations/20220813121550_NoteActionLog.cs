using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Papyrus.Migrations
{
    public partial class NoteActionLog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NoteActionLogs_Notes_NoteId",
                table: "NoteActionLogs");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "NoteActionLogs",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<DateTime>(
                name: "Creation",
                table: "NoteActionLogs",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "PerformerId",
                table: "NoteActionLogs",
                type: "varchar(255)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Text",
                table: "NoteActionLogs",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "NoteActionLogs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_NoteActionLogs_PerformerId",
                table: "NoteActionLogs",
                column: "PerformerId");

            migrationBuilder.AddForeignKey(
                name: "FK_NoteActionLogs_AspNetUsers_PerformerId",
                table: "NoteActionLogs",
                column: "PerformerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_NoteActionLogs_Notes_NoteId",
                table: "NoteActionLogs",
                column: "NoteId",
                principalTable: "Notes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NoteActionLogs_AspNetUsers_PerformerId",
                table: "NoteActionLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_NoteActionLogs_Notes_NoteId",
                table: "NoteActionLogs");

            migrationBuilder.DropIndex(
                name: "IX_NoteActionLogs_PerformerId",
                table: "NoteActionLogs");

            migrationBuilder.DropColumn(
                name: "Creation",
                table: "NoteActionLogs");

            migrationBuilder.DropColumn(
                name: "PerformerId",
                table: "NoteActionLogs");

            migrationBuilder.DropColumn(
                name: "Text",
                table: "NoteActionLogs");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "NoteActionLogs");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "NoteActionLogs",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddForeignKey(
                name: "FK_NoteActionLogs_Notes_NoteId",
                table: "NoteActionLogs",
                column: "NoteId",
                principalTable: "Notes",
                principalColumn: "Id");
        }
    }
}
