using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaekwondoBackend.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "korisnici",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    password_hash = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    ime = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    telefon = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    uloga = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    stanje = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "na_cekanju"),
                    pojas = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "bijeli"),
                    datum_pridruzivanja = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    napomene = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_korisnici", x => x.id);
                    table.CheckConstraint("CK_Korisnici_Pojas", "pojas IN ('bijeli', 'zuti', 'narancasti', 'zeleni', 'plavi', 'smedi', 'crni_1_dan', 'crni_2_dan', 'crni_3_dan', 'crni_4_dan', 'crni_5_dan', 'crni_6_dan', 'crni_7_dan', 'crni_8_dan', 'crni_9_dan', 'crni_10_dan')");
                    table.CheckConstraint("CK_Korisnici_Stanje", "stanje IN ('aktivan', 'neaktivan', 'na_cekanju')");
                    table.CheckConstraint("CK_Korisnici_Uloga", "uloga IN ('administrator', 'trener', 'ucenik')");
                });

            migrationBuilder.CreateTable(
                name: "objave",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    naslov = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    sadrzaj = table.Column<string>(type: "text", nullable: false),
                    tip = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    image_url = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    autor_id = table.Column<Guid>(type: "uuid", nullable: false),
                    objavljeno = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_objave", x => x.id);
                    table.CheckConstraint("CK_Objave_Tip", "tip IN ('vijesti', 'obavjestenje')");
                    table.ForeignKey(
                        name: "FK_objave_korisnici_autor_id",
                        column: x => x.autor_id,
                        principalTable: "korisnici",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_korisnici_email",
                table: "korisnici",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_korisnici_pojas",
                table: "korisnici",
                column: "pojas");

            migrationBuilder.CreateIndex(
                name: "IX_korisnici_stanje",
                table: "korisnici",
                column: "stanje");

            migrationBuilder.CreateIndex(
                name: "IX_korisnici_uloga",
                table: "korisnici",
                column: "uloga");

            migrationBuilder.CreateIndex(
                name: "IX_objave_autor_id",
                table: "objave",
                column: "autor_id");

            migrationBuilder.CreateIndex(
                name: "IX_objave_objavljeno",
                table: "objave",
                column: "objavljeno");

            migrationBuilder.CreateIndex(
                name: "IX_objave_tip",
                table: "objave",
                column: "tip");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "objave");

            migrationBuilder.DropTable(
                name: "korisnici");
        }
    }
}
