using Microsoft.EntityFrameworkCore;
using TaekwondoBackend.Entities;
using TaekwondoBackend.Models;

namespace TaekwondoBackend.Data
{
    public class TaekwondoDbContext(DbContextOptions<TaekwondoDbContext> options) : DbContext(options)
    {
        public DbSet<Belts> Belts => Set<Belts>();
        public DbSet<User> Users { get; set; }
        public DbSet<Members> Members { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed data for the Belts table
            modelBuilder.Entity<Belts>().HasData(
                new Belts { Id = 1, Name = "White", SequenceNumber = 1 },
                new Belts { Id = 2, Name = "Yellow", SequenceNumber = 2 },
                new Belts { Id = 3, Name = "Green", SequenceNumber = 3 },
                new Belts { Id = 4, Name = "Blue", SequenceNumber = 4 },
                new Belts { Id = 5, Name = "Red", SequenceNumber = 5 },
                new Belts { Id = 6, Name = "Black", SequenceNumber = 6 }
            );
        }


    }

}
