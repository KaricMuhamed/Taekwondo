using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaekwondoBackend.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.CreateTable(
                name: "grupe",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    naziv = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    opis = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_grupe", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "korisnici",
                schema: "public",
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
                name: "treninzi",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    naziv = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    opis = table.Column<string>(type: "text", nullable: false),
                    datum = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    vrijeme_od = table.Column<TimeSpan>(type: "interval", nullable: false),
                    vrijeme_do = table.Column<TimeSpan>(type: "interval", nullable: false),
                    lokacija = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_treninzi", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "grupa_ucenici",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    grupa_id = table.Column<Guid>(type: "uuid", nullable: false),
                    ucenik_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_grupa_ucenici", x => x.id);
                    table.ForeignKey(
                        name: "FK_grupa_ucenici_grupe_grupa_id",
                        column: x => x.grupa_id,
                        principalSchema: "public",
                        principalTable: "grupe",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_grupa_ucenici_korisnici_ucenik_id",
                        column: x => x.ucenik_id,
                        principalSchema: "public",
                        principalTable: "korisnici",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "objave",
                schema: "public",
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
                        principalSchema: "public",
                        principalTable: "korisnici",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "trening_grupe",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    trening_id = table.Column<Guid>(type: "uuid", nullable: false),
                    grupa_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_trening_grupe", x => x.id);
                    table.ForeignKey(
                        name: "FK_trening_grupe_grupe_grupa_id",
                        column: x => x.grupa_id,
                        principalSchema: "public",
                        principalTable: "grupe",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_trening_grupe_treninzi_trening_id",
                        column: x => x.trening_id,
                        principalSchema: "public",
                        principalTable: "treninzi",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "trening_treneri",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    trening_id = table.Column<Guid>(type: "uuid", nullable: false),
                    trener_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_trening_treneri", x => x.id);
                    table.ForeignKey(
                        name: "FK_trening_treneri_korisnici_trener_id",
                        column: x => x.trener_id,
                        principalSchema: "public",
                        principalTable: "korisnici",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_trening_treneri_treninzi_trening_id",
                        column: x => x.trening_id,
                        principalSchema: "public",
                        principalTable: "treninzi",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "public",
                table: "korisnici",
                columns: new[] { "id", "created_at", "datum_pridruzivanja", "email", "ime", "napomene", "password_hash", "pojas", "stanje", "telefon", "uloga", "updated_at" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "admin@admin.com", "Admin", "Administrator user", "$2a$12$SQbY2HKI4X/1ytaMCHSGcOWVr7pWZ128jgdwhNRh41vZ7w67bhcNO", "crni_1_dan", "aktivan", "+38761234567", "administrator", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.CreateIndex(
                name: "IX_grupa_ucenici_grupa_id_ucenik_id",
                schema: "public",
                table: "grupa_ucenici",
                columns: new[] { "grupa_id", "ucenik_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_grupa_ucenici_ucenik_id",
                schema: "public",
                table: "grupa_ucenici",
                column: "ucenik_id");

            migrationBuilder.CreateIndex(
                name: "IX_grupe_naziv",
                schema: "public",
                table: "grupe",
                column: "naziv");

            migrationBuilder.CreateIndex(
                name: "IX_korisnici_email",
                schema: "public",
                table: "korisnici",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_korisnici_pojas",
                schema: "public",
                table: "korisnici",
                column: "pojas");

            migrationBuilder.CreateIndex(
                name: "IX_korisnici_stanje",
                schema: "public",
                table: "korisnici",
                column: "stanje");

            migrationBuilder.CreateIndex(
                name: "IX_korisnici_uloga",
                schema: "public",
                table: "korisnici",
                column: "uloga");

            migrationBuilder.CreateIndex(
                name: "IX_objave_autor_id",
                schema: "public",
                table: "objave",
                column: "autor_id");

            migrationBuilder.CreateIndex(
                name: "IX_objave_objavljeno",
                schema: "public",
                table: "objave",
                column: "objavljeno");

            migrationBuilder.CreateIndex(
                name: "IX_objave_tip",
                schema: "public",
                table: "objave",
                column: "tip");

            migrationBuilder.CreateIndex(
                name: "IX_trening_grupe_grupa_id",
                schema: "public",
                table: "trening_grupe",
                column: "grupa_id");

            migrationBuilder.CreateIndex(
                name: "IX_trening_grupe_trening_id_grupa_id",
                schema: "public",
                table: "trening_grupe",
                columns: new[] { "trening_id", "grupa_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_trening_treneri_trener_id",
                schema: "public",
                table: "trening_treneri",
                column: "trener_id");

            migrationBuilder.CreateIndex(
                name: "IX_trening_treneri_trening_id_trener_id",
                schema: "public",
                table: "trening_treneri",
                columns: new[] { "trening_id", "trener_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_treninzi_datum",
                schema: "public",
                table: "treninzi",
                column: "datum");

            migrationBuilder.CreateIndex(
                name: "IX_treninzi_naziv",
                schema: "public",
                table: "treninzi",
                column: "naziv");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "grupa_ucenici",
                schema: "public");

            migrationBuilder.DropTable(
                name: "objave",
                schema: "public");

            migrationBuilder.DropTable(
                name: "trening_grupe",
                schema: "public");

            migrationBuilder.DropTable(
                name: "trening_treneri",
                schema: "public");

            migrationBuilder.DropTable(
                name: "grupe",
                schema: "public");

            migrationBuilder.DropTable(
                name: "korisnici",
                schema: "public");

            migrationBuilder.DropTable(
                name: "treninzi",
                schema: "public");
        }
    }
}
