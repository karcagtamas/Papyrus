using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Papyrus.Migrations
{
    public partial class GlobalActionLog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GroupActionLogs");

            migrationBuilder.CreateTable(
                name: "ActionLogs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Key = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Segment = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Language = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Type = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Text = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Creation = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    PerformerId = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActionLogs_AspNetUsers_PerformerId",
                        column: x => x.PerformerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Translations",
                columns: new[] { "Key", "Language", "Segment", "Value" },
                values: new object[,]
                {
                    { "Close", "en-US", "Group", "Group closed" },
                    { "Close", "hu-HU", "Group", "Group closed" },
                    { "Create", "en-US", "Group", "Group created" },
                    { "Create", "hu-HU", "Group", "Group created" },
                    { "DataEdit", "en-US", "Group", "Data edited" },
                    { "DataEdit", "hu-HU", "Group", "Data edited" },
                    { "MemberAdd", "en-US", "Group", "Member added" },
                    { "MemberAdd", "hu-HU", "Group", "Member added" },
                    { "MemberEdit", "en-US", "Group", "Member edited" },
                    { "MemberEdit", "hu-HU", "Group", "Member edited" },
                    { "MemberRemove", "en-US", "Group", "Member removed" },
                    { "MemberRemove", "hu-HU", "Group", "Member removed" },
                    { "NoteCreate", "en-US", "Group", "Note created" },
                    { "NoteCreate", "hu-HU", "Group", "Note created" },
                    { "Open", "en-US", "Group", "Group opened" },
                    { "Open", "hu-HU", "Group", "Group opened" },
                    { "RoleCreate", "en-US", "Group", "Role created" },
                    { "RoleCreate", "hu-HU", "Group", "Role created" },
                    { "RoleEdit", "en-US", "Group", "Role edited" },
                    { "RoleEdit", "hu-HU", "Group", "Role edited" },
                    { "RoleRemove", "en-US", "Group", "Role removed" },
                    { "RoleRemove", "hu-HU", "Group", "Role removed" },
                    { "TagCreate", "en-US", "Group", "Tag created" },
                    { "TagCreate", "hu-HU", "Group", "Tag created" },
                    { "TagEdite", "en-US", "Group", "Tag edited" },
                    { "TagEdite", "hu-HU", "Group", "Tag edited" },
                    { "TagRemove", "en-US", "Group", "Tag removed" },
                    { "TagRemove", "hu-HU", "Group", "Tag removed" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActionLogs_PerformerId",
                table: "ActionLogs",
                column: "PerformerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActionLogs");

            migrationBuilder.DeleteData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "Close", "en-US", "Group" });

            migrationBuilder.DeleteData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "Close", "hu-HU", "Group" });

            migrationBuilder.DeleteData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "Create", "en-US", "Group" });

            migrationBuilder.DeleteData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "Create", "hu-HU", "Group" });

            migrationBuilder.DeleteData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "DataEdit", "en-US", "Group" });

            migrationBuilder.DeleteData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "DataEdit", "hu-HU", "Group" });

            migrationBuilder.DeleteData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "MemberAdd", "en-US", "Group" });

            migrationBuilder.DeleteData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "MemberAdd", "hu-HU", "Group" });

            migrationBuilder.DeleteData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "MemberEdit", "en-US", "Group" });

            migrationBuilder.DeleteData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "MemberEdit", "hu-HU", "Group" });

            migrationBuilder.DeleteData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "MemberRemove", "en-US", "Group" });

            migrationBuilder.DeleteData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "MemberRemove", "hu-HU", "Group" });

            migrationBuilder.DeleteData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "NoteCreate", "en-US", "Group" });

            migrationBuilder.DeleteData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "NoteCreate", "hu-HU", "Group" });

            migrationBuilder.DeleteData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "Open", "en-US", "Group" });

            migrationBuilder.DeleteData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "Open", "hu-HU", "Group" });

            migrationBuilder.DeleteData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "RoleCreate", "en-US", "Group" });

            migrationBuilder.DeleteData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "RoleCreate", "hu-HU", "Group" });

            migrationBuilder.DeleteData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "RoleEdit", "en-US", "Group" });

            migrationBuilder.DeleteData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "RoleEdit", "hu-HU", "Group" });

            migrationBuilder.DeleteData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "RoleRemove", "en-US", "Group" });

            migrationBuilder.DeleteData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "RoleRemove", "hu-HU", "Group" });

            migrationBuilder.DeleteData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "TagCreate", "en-US", "Group" });

            migrationBuilder.DeleteData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "TagCreate", "hu-HU", "Group" });

            migrationBuilder.DeleteData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "TagEdite", "en-US", "Group" });

            migrationBuilder.DeleteData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "TagEdite", "hu-HU", "Group" });

            migrationBuilder.DeleteData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "TagRemove", "en-US", "Group" });

            migrationBuilder.DeleteData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "TagRemove", "hu-HU", "Group" });

            migrationBuilder.CreateTable(
                name: "GroupActionLogs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    PerformerId = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Creation = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Text = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupActionLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroupActionLogs_AspNetUsers_PerformerId",
                        column: x => x.PerformerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_GroupActionLogs_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_GroupActionLogs_GroupId",
                table: "GroupActionLogs",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupActionLogs_PerformerId",
                table: "GroupActionLogs",
                column: "PerformerId");
        }
    }
}
