using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Papyrus.Migrations
{
    public partial class NoteLogs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NoteActionLogs");

            migrationBuilder.UpdateData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "Close", "en-US", "Group" },
                column: "Value",
                value: "Closed");

            migrationBuilder.UpdateData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "Create", "en-US", "Group" },
                column: "Value",
                value: "Created");

            migrationBuilder.UpdateData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "DataEdit", "en-US", "Group" },
                column: "Value",
                value: "Data is edited");

            migrationBuilder.UpdateData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "MemberAdd", "en-US", "Group" },
                column: "Value",
                value: "Member is added");

            migrationBuilder.UpdateData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "MemberEdit", "en-US", "Group" },
                column: "Value",
                value: "Member is edited");

            migrationBuilder.UpdateData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "MemberRemove", "en-US", "Group" },
                column: "Value",
                value: "Member is removed");

            migrationBuilder.UpdateData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "NoteCreate", "en-US", "Group" },
                column: "Value",
                value: "Note is created");

            migrationBuilder.UpdateData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "Open", "en-US", "Group" },
                column: "Value",
                value: "Opened");

            migrationBuilder.UpdateData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "RoleCreate", "en-US", "Group" },
                column: "Value",
                value: "Role is created");

            migrationBuilder.UpdateData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "RoleEdit", "en-US", "Group" },
                column: "Value",
                value: "Role is edited");

            migrationBuilder.UpdateData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "RoleRemove", "en-US", "Group" },
                column: "Value",
                value: "Role is removed");

            migrationBuilder.UpdateData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "TagCreate", "en-US", "Group" },
                column: "Value",
                value: "Tag is created");

            migrationBuilder.UpdateData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "TagEdite", "en-US", "Group" },
                column: "Value",
                value: "Tag is edited");

            migrationBuilder.UpdateData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "TagRemove", "en-US", "Group" },
                column: "Value",
                value: "Tag is removed");

            migrationBuilder.InsertData(
                table: "Translations",
                columns: new[] { "Key", "Language", "Segment", "Value" },
                values: new object[,]
                {
                    { "ContentEdit", "en-US", "Note", "Content is edited" },
                    { "ContentEdit", "hu-HU", "Note", "Tartalom szerkesztve" },
                    { "Create", "en-US", "Note", "Created" },
                    { "Create", "hu-HU", "Note", "Létrehozva" },
                    { "Delete", "en-US", "Note", "Deleted" },
                    { "Delete", "hu-HU", "Note", "Törölve" },
                    { "Publish", "en-US", "Note", "Public status is changed" },
                    { "Publish", "hu-HU", "Note", "Nyílvános státusz megváltoztatva" },
                    { "TagEdit", "en-US", "Note", "Tag(s) added or removed" },
                    { "TagEdit", "hu-HU", "Note", "Címke/Címkék hozzáadva vagy törölve" },
                    { "TitleEdit", "en-US", "Note", "Title is edited" },
                    { "TitleEdit", "hu-HU", "Note", "Cím szerkesztve" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "ContentEdit", "en-US", "Note" });

            migrationBuilder.DeleteData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "ContentEdit", "hu-HU", "Note" });

            migrationBuilder.DeleteData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "Create", "en-US", "Note" });

            migrationBuilder.DeleteData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "Create", "hu-HU", "Note" });

            migrationBuilder.DeleteData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "Delete", "en-US", "Note" });

            migrationBuilder.DeleteData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "Delete", "hu-HU", "Note" });

            migrationBuilder.DeleteData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "Publish", "en-US", "Note" });

            migrationBuilder.DeleteData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "Publish", "hu-HU", "Note" });

            migrationBuilder.DeleteData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "TagEdit", "en-US", "Note" });

            migrationBuilder.DeleteData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "TagEdit", "hu-HU", "Note" });

            migrationBuilder.DeleteData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "TitleEdit", "en-US", "Note" });

            migrationBuilder.DeleteData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "TitleEdit", "hu-HU", "Note" });

            migrationBuilder.CreateTable(
                name: "NoteActionLogs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NoteId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PerformerId = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Creation = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Text = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NoteActionLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NoteActionLogs_AspNetUsers_PerformerId",
                        column: x => x.PerformerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_NoteActionLogs_Notes_NoteId",
                        column: x => x.NoteId,
                        principalTable: "Notes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "Close", "en-US", "Group" },
                column: "Value",
                value: "Group closed");

            migrationBuilder.UpdateData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "Create", "en-US", "Group" },
                column: "Value",
                value: "Group created");

            migrationBuilder.UpdateData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "DataEdit", "en-US", "Group" },
                column: "Value",
                value: "Data edited");

            migrationBuilder.UpdateData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "MemberAdd", "en-US", "Group" },
                column: "Value",
                value: "Member added");

            migrationBuilder.UpdateData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "MemberEdit", "en-US", "Group" },
                column: "Value",
                value: "Member edited");

            migrationBuilder.UpdateData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "MemberRemove", "en-US", "Group" },
                column: "Value",
                value: "Member removed");

            migrationBuilder.UpdateData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "NoteCreate", "en-US", "Group" },
                column: "Value",
                value: "Note created");

            migrationBuilder.UpdateData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "Open", "en-US", "Group" },
                column: "Value",
                value: "Group opened");

            migrationBuilder.UpdateData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "RoleCreate", "en-US", "Group" },
                column: "Value",
                value: "Role created");

            migrationBuilder.UpdateData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "RoleEdit", "en-US", "Group" },
                column: "Value",
                value: "Role edited");

            migrationBuilder.UpdateData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "RoleRemove", "en-US", "Group" },
                column: "Value",
                value: "Role removed");

            migrationBuilder.UpdateData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "TagCreate", "en-US", "Group" },
                column: "Value",
                value: "Tag created");

            migrationBuilder.UpdateData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "TagEdite", "en-US", "Group" },
                column: "Value",
                value: "Tag edited");

            migrationBuilder.UpdateData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "TagRemove", "en-US", "Group" },
                column: "Value",
                value: "Tag removed");

            migrationBuilder.CreateIndex(
                name: "IX_NoteActionLogs_NoteId",
                table: "NoteActionLogs",
                column: "NoteId");

            migrationBuilder.CreateIndex(
                name: "IX_NoteActionLogs_PerformerId",
                table: "NoteActionLogs",
                column: "PerformerId");
        }
    }
}
