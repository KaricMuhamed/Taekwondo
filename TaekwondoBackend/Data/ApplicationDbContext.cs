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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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
        }
    }
}
