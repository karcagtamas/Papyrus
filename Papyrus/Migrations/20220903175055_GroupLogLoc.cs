using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Papyrus.Migrations
{
    public partial class GroupLogLoc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "Close", "hu-HU", "Group" },
                column: "Value",
                value: "Csoport lezárva");

            migrationBuilder.UpdateData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "Create", "hu-HU", "Group" },
                column: "Value",
                value: "Csoport létrehozva");

            migrationBuilder.UpdateData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "DataEdit", "hu-HU", "Group" },
                column: "Value",
                value: "Adatok szerkesztve");

            migrationBuilder.UpdateData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "MemberAdd", "hu-HU", "Group" },
                column: "Value",
                value: "Tag hozzáadva");

            migrationBuilder.UpdateData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "MemberEdit", "hu-HU", "Group" },
                column: "Value",
                value: "Tag szerkesztve");

            migrationBuilder.UpdateData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "MemberRemove", "hu-HU", "Group" },
                column: "Value",
                value: "Tag törölve");

            migrationBuilder.UpdateData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "NoteCreate", "hu-HU", "Group" },
                column: "Value",
                value: "Jegyzet létrehozva");

            migrationBuilder.UpdateData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "Open", "hu-HU", "Group" },
                column: "Value",
                value: "Csoport kinyitva");

            migrationBuilder.UpdateData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "RoleCreate", "hu-HU", "Group" },
                column: "Value",
                value: "Szerepkör létrehozva");

            migrationBuilder.UpdateData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "RoleEdit", "hu-HU", "Group" },
                column: "Value",
                value: "Szerepkör szerkesztve");

            migrationBuilder.UpdateData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "RoleRemove", "hu-HU", "Group" },
                column: "Value",
                value: "Szerepkör törölve");

            migrationBuilder.UpdateData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "TagCreate", "hu-HU", "Group" },
                column: "Value",
                value: "Címke létrehozva");

            migrationBuilder.UpdateData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "TagEdite", "hu-HU", "Group" },
                column: "Value",
                value: "Címke szerkesztve");

            migrationBuilder.UpdateData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "TagRemove", "hu-HU", "Group" },
                column: "Value",
                value: "Címke törölve");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "Close", "hu-HU", "Group" },
                column: "Value",
                value: "Group closed");

            migrationBuilder.UpdateData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "Create", "hu-HU", "Group" },
                column: "Value",
                value: "Group created");

            migrationBuilder.UpdateData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "DataEdit", "hu-HU", "Group" },
                column: "Value",
                value: "Data edited");

            migrationBuilder.UpdateData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "MemberAdd", "hu-HU", "Group" },
                column: "Value",
                value: "Member added");

            migrationBuilder.UpdateData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "MemberEdit", "hu-HU", "Group" },
                column: "Value",
                value: "Member edited");

            migrationBuilder.UpdateData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "MemberRemove", "hu-HU", "Group" },
                column: "Value",
                value: "Member removed");

            migrationBuilder.UpdateData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "NoteCreate", "hu-HU", "Group" },
                column: "Value",
                value: "Note created");

            migrationBuilder.UpdateData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "Open", "hu-HU", "Group" },
                column: "Value",
                value: "Group opened");

            migrationBuilder.UpdateData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "RoleCreate", "hu-HU", "Group" },
                column: "Value",
                value: "Role created");

            migrationBuilder.UpdateData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "RoleEdit", "hu-HU", "Group" },
                column: "Value",
                value: "Role edited");

            migrationBuilder.UpdateData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "RoleRemove", "hu-HU", "Group" },
                column: "Value",
                value: "Role removed");

            migrationBuilder.UpdateData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "TagCreate", "hu-HU", "Group" },
                column: "Value",
                value: "Tag created");

            migrationBuilder.UpdateData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "TagEdite", "hu-HU", "Group" },
                column: "Value",
                value: "Tag edited");

            migrationBuilder.UpdateData(
                table: "Translations",
                keyColumns: new[] { "Key", "Language", "Segment" },
                keyValues: new object[] { "TagRemove", "hu-HU", "Group" },
                column: "Value",
                value: "Tag removed");
        }
    }
}
