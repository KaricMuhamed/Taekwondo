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
        public DbSet<UserMember> UserMembers { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupMember> GroupMembers { get; set; }
        public DbSet<TrainingSession> TrainingSessions { get; set; }
        public DbSet<TrainingSessionMember> TrainingSessionMembers { get; set; }

        public DbSet<Tournament> Tournaments { get; set; }
        public DbSet<TournamentMember> TournamentMembers { get; set; }
        public DbSet<BeltTest> BeltTests { get; set; }
        public DbSet<Post> Posts { get; set; }


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

            modelBuilder.Entity<UserMember>()
                .HasKey(um => new { um.UserId, um.MemberId });

            modelBuilder.Entity<UserMember>()
                .HasOne(um => um.User)
                .WithMany(u => u.UserMembers)
                .HasForeignKey(um => um.UserId);

            modelBuilder.Entity<UserMember>()
                .HasOne(um => um.Member)
                .WithMany(m => m.UserMembers)
                .HasForeignKey(um => um.MemberId);

            modelBuilder.Entity<GroupMember>()
            .HasKey(gm => new { gm.GroupId, gm.MemberId });

            modelBuilder.Entity<GroupMember>()
                .HasOne(gm => gm.Group)
                .WithMany(g => g.GroupMembers)
                .HasForeignKey(gm => gm.GroupId);

            modelBuilder.Entity<GroupMember>()
                .HasOne(gm => gm.Member)
                .WithMany()
                .HasForeignKey(gm => gm.MemberId);

            modelBuilder.Entity<TrainingSessionMember>()
            .HasKey(tsm => new { tsm.TrainingSessionId, tsm.MemberId });

            modelBuilder.Entity<TrainingSessionMember>()
                .HasOne(tsm => tsm.TrainingSession)
                .WithMany(ts => ts.TrainingSessionMembers)
                .HasForeignKey(tsm => tsm.TrainingSessionId);

            modelBuilder.Entity<TrainingSessionMember>()
                .HasOne(tsm => tsm.Member)
                .WithMany()
                .HasForeignKey(tsm => tsm.MemberId);

            modelBuilder.Entity<TrainingSession>()
                .HasOne(ts => ts.Group)
                .WithMany()
                .HasForeignKey(ts => ts.GroupId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<TournamentMember>()
               .HasOne(tm => tm.Tournament)
               .WithMany(t => t.TournamentMembers)
               .HasForeignKey(tm => tm.TournamentId);

            modelBuilder.Entity<TournamentMember>()
                .HasOne(tm => tm.Member)
                .WithMany()
                .HasForeignKey(tm => tm.MemberId);

            modelBuilder.Entity<BeltTest>(entity =>
            {
                entity.HasKey(bt => bt.Id);

                entity.Property(bt => bt.ScheduledDate).IsRequired();

                entity.Property(bt => bt.Status)
                      .HasConversion<string>()
                      .IsRequired();

                entity.HasOne(bt => bt.Member)
                      .WithMany()
                      .HasForeignKey(bt => bt.MemberId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(bt => bt.CurrentBelt)
                      .WithMany()
                      .HasForeignKey(bt => bt.CurrentBeltId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(bt => bt.AppliedBelt)
                      .WithMany()
                      .HasForeignKey(bt => bt.AppliedBeltId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Post>()
                .HasOne(p => p.User)
                .WithMany(u => u.Posts)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);


        }


    }

}
