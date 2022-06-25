using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Papyrus.Migrations
{
    public partial class TagFixCheck : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Tag_Owner",
                table: "Tags");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Tag_Owner",
                table: "Tags",
                sql: "(GroupId IS NOT NULL OR UserId IS NOT NULL) AND NOT (GroupId IS NOT NULL AND UserId IS NOT NULL)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Tag_Owner",
                table: "Tags");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Tag_Owner",
                table: "Tags",
                sql: "(GroupId IS NOT NULL OR UserId IS NOT NULL) AND (GroupId IS NOT NULL AND UserId IS NOT NULL)");
        }
    }
}
