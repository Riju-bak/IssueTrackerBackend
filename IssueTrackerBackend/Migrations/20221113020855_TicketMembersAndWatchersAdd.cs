using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IssueTrackerBackend.Migrations
{
    public partial class TicketMembersAndWatchersAdd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TicketId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TicketId1",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_TicketId",
                table: "Users",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_TicketId1",
                table: "Users",
                column: "TicketId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Tickets_TicketId",
                table: "Users",
                column: "TicketId",
                principalTable: "Tickets",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Tickets_TicketId1",
                table: "Users",
                column: "TicketId1",
                principalTable: "Tickets",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Tickets_TicketId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Tickets_TicketId1",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_TicketId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_TicketId1",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TicketId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TicketId1",
                table: "Users");
        }
    }
}
