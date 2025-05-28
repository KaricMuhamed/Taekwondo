using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaekwondoBackend.Migrations
{
    /// <inheritdoc />
    public partial class belttests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BeltTests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemberId = table.Column<int>(type: "int", nullable: false),
                    CurrentBeltId = table.Column<int>(type: "int", nullable: false),
                    AppliedBeltId = table.Column<int>(type: "int", nullable: false),
                    ScheduledDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TestedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BeltTests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BeltTests_Belts_AppliedBeltId",
                        column: x => x.AppliedBeltId,
                        principalTable: "Belts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BeltTests_Belts_CurrentBeltId",
                        column: x => x.CurrentBeltId,
                        principalTable: "Belts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BeltTests_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BeltTests_AppliedBeltId",
                table: "BeltTests",
                column: "AppliedBeltId");

            migrationBuilder.CreateIndex(
                name: "IX_BeltTests_CurrentBeltId",
                table: "BeltTests",
                column: "CurrentBeltId");

            migrationBuilder.CreateIndex(
                name: "IX_BeltTests_MemberId",
                table: "BeltTests",
                column: "MemberId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BeltTests");
        }
    }
}
