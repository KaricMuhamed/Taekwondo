using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaekwondoBackend.Migrations
{
    /// <inheritdoc />
    public partial class update_ts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GroupId",
                table: "TrainingSessions",
                type: "int",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TrainingSessions_Groups_GroupId",
                table: "TrainingSessions",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrainingSessions_Groups_GroupId",
                table: "TrainingSessions");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "TrainingSessions");
        }
    }
}
