using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IssueTrackerBackend.Migrations
{
    public partial class PropDueDateNameFixed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DueData",
                table: "Tickets",
                newName: "DueDate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DueDate",
                table: "Tickets",
                newName: "DueData");
        }
    }
}
