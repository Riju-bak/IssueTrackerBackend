using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IssueTrackerBackend.Migrations
{
    public partial class Workspaces : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WorkspaceId",
                table: "Boards",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Workspaces",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workspaces", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserWorkspace",
                columns: table => new
                {
                    MembersId = table.Column<int>(type: "int", nullable: false),
                    WorkspacesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserWorkspace", x => new { x.MembersId, x.WorkspacesId });
                    table.ForeignKey(
                        name: "FK_UserWorkspace_Users_MembersId",
                        column: x => x.MembersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserWorkspace_Workspaces_WorkspacesId",
                        column: x => x.WorkspacesId,
                        principalTable: "Workspaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Boards_WorkspaceId",
                table: "Boards",
                column: "WorkspaceId");

            migrationBuilder.CreateIndex(
                name: "IX_UserWorkspace_WorkspacesId",
                table: "UserWorkspace",
                column: "WorkspacesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Boards_Workspaces_WorkspaceId",
                table: "Boards",
                column: "WorkspaceId",
                principalTable: "Workspaces",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Boards_Workspaces_WorkspaceId",
                table: "Boards");

            migrationBuilder.DropTable(
                name: "UserWorkspace");

            migrationBuilder.DropTable(
                name: "Workspaces");

            migrationBuilder.DropIndex(
                name: "IX_Boards_WorkspaceId",
                table: "Boards");

            migrationBuilder.DropColumn(
                name: "WorkspaceId",
                table: "Boards");
        }
    }
}
