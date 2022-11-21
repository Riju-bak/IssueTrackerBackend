using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IssueTrackerBackend.Migrations
{
    public partial class BoardTicketRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BoardId",
                table: "Tickets",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Boards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Boards", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_BoardId",
                table: "Tickets",
                column: "BoardId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Boards_BoardId",
                table: "Tickets",
                column: "BoardId",
                principalTable: "Boards",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Boards_BoardId",
                table: "Tickets");

            migrationBuilder.DropTable(
                name: "Boards");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_BoardId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "BoardId",
                table: "Tickets");
        }
    }
}
