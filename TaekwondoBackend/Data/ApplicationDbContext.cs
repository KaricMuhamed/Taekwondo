using Microsoft.EntityFrameworkCore;
using TaekwondoBackend.Entities;

namespace TaekwondoBackend.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Korisnik> Korisnici { get; set; }
        public DbSet<Objava> Objave { get; set; }
        public DbSet<Grupa> Grupe { get; set; }
        public DbSet<Trening> Treninzi { get; set; }
        public DbSet<GrupaUcenik> GrupaUcenici { get; set; }
        public DbSet<TreningTrener> TreningTreneri { get; set; }
        public DbSet<TreningGrupa> TreningGrupe { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("public");
            base.OnModelCreating(modelBuilder);

            // Configure Korisnik entity
            modelBuilder.Entity<Korisnik>(entity =>
            {
                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasIndex(e => e.Uloga);
                entity.HasIndex(e => e.Stanje);
                entity.HasIndex(e => e.Pojas);

                entity.Property(e => e.Stanje).HasDefaultValue("na_cekanju");
                entity.Property(e => e.Pojas).HasDefaultValue("bijeli");

                entity.HasCheckConstraint("CK_Korisnici_Uloga",
                    "uloga IN ('administrator', 'trener', 'ucenik')");
                entity.HasCheckConstraint("CK_Korisnici_Stanje",
                    "stanje IN ('aktivan', 'neaktivan', 'na_cekanju')");
                entity.HasCheckConstraint("CK_Korisnici_Pojas",
                    "pojas IN ('bijeli', 'zuti', 'narancasti', 'zeleni', 'plavi', 'smedi', " +
                    "'crni_1_dan', 'crni_2_dan', 'crni_3_dan', 'crni_4_dan', 'crni_5_dan', " +
                    "'crni_6_dan', 'crni_7_dan', 'crni_8_dan', 'crni_9_dan', 'crni_10_dan')");
            });

            // Seed with administrator user
            modelBuilder.Entity<Korisnik>().HasData(new Korisnik
            {
                Id = new Guid("00000000-0000-0000-0000-000000000001"),
                Ime = "Admin",
                Email = "admin@admin.com",
                PasswordHash = "$2a$12$SQbY2HKI4X/1ytaMCHSGcOWVr7pWZ128jgdwhNRh41vZ7w67bhcNO",
                Uloga = "administrator",
                Stanje = "aktivan",
                Pojas = "crni_1_dan",
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                DatumPridruzivanja = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                Napomene = "Administrator user",
                Telefon = "+38761234567"
            });
            // Configure Objava entity
            modelBuilder.Entity<Objava>(entity =>
            {
                entity.HasIndex(e => e.Tip);
                entity.HasIndex(e => e.AutorId);
                entity.HasIndex(e => e.Objavljeno);

                entity.HasCheckConstraint("CK_Objave_Tip", "tip IN ('vijesti', 'obavjestenje')");

                entity.HasOne(o => o.Autor)
                      .WithMany(k => k.AutorObjave)
                      .HasForeignKey(o => o.AutorId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure Grupa entity
            modelBuilder.Entity<Grupa>(entity =>
            {
                entity.HasIndex(e => e.Naziv);
            });

            // Configure Trening entity
            modelBuilder.Entity<Trening>(entity =>
            {
                entity.HasIndex(e => e.Datum);
                entity.HasIndex(e => e.Naziv);
            });

            // Configure GrupaUcenik junction table
            modelBuilder.Entity<GrupaUcenik>(entity =>
            {
                entity.HasIndex(e => new { e.GrupaId, e.UcenikId }).IsUnique();

                entity.HasOne(gu => gu.Grupa)
                    .WithMany(g => g.GrupaUcenici)
                    .HasForeignKey(gu => gu.GrupaId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(gu => gu.Ucenik)
                    .WithMany()
                    .HasForeignKey(gu => gu.UcenikId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure TreningTrener junction table
            modelBuilder.Entity<TreningTrener>(entity =>
            {
                entity.HasIndex(e => new { e.TreningId, e.TrenerId }).IsUnique();

                entity.HasOne(tt => tt.Trening)
                    .WithMany(t => t.TreningTreneri)
                    .HasForeignKey(tt => tt.TreningId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(tt => tt.Trener)
                    .WithMany()
                    .HasForeignKey(tt => tt.TrenerId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure TreningGrupa junction table
            modelBuilder.Entity<TreningGrupa>(entity =>
            {
                entity.HasIndex(e => new { e.TreningId, e.GrupaId }).IsUnique();

                entity.HasOne(tg => tg.Trening)
                    .WithMany(t => t.TreningGrupe)
                    .HasForeignKey(tg => tg.TreningId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(tg => tg.Grupa)
                    .WithMany(g => g.TreningGrupe)
                    .HasForeignKey(tg => tg.GrupaId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
