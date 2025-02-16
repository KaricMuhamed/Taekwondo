using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TaekwondoBackend.Migrations
{
    /// <inheritdoc />
    public partial class SeedBelts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Belts",
                columns: new[] { "Id", "Name", "SequenceNumber" },
                values: new object[,]
                {
                    { 1, "White", 1 },
                    { 2, "Yellow", 2 },
                    { 3, "Green", 3 },
                    { 4, "Blue", 4 },
                    { 5, "Red", 5 },
                    { 6, "Black", 6 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Belts",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Belts",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Belts",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Belts",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Belts",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Belts",
                keyColumn: "Id",
                keyValue: 6);
        }
    }
}
