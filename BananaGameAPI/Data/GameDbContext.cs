using Microsoft.EntityFrameworkCore;
using BananaGameAPI.Models;

namespace BananaGameAPI.Data
{
    public class GameDbContext : DbContext
    {
        public GameDbContext(DbContextOptions<GameDbContext> options) : base(options) { }

        public DbSet<Player> Players { get; set; }
        public DbSet<Score> Scores { get; set; }
        public DbSet<GameResult> GameResults { get; set; } // ✅ Added GameResults DbSet

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ✅ Ensure Username and Email are Unique
            modelBuilder.Entity<Player>()
                .HasIndex(p => p.Username)
                .IsUnique();

            modelBuilder.Entity<Player>()
                .HasIndex(p => p.Email)
                .IsUnique();

            // ✅ Define One-to-Many Relationship (Player → Scores)
            modelBuilder.Entity<Score>()
                .HasOne(s => s.Player)
                .WithMany(p => p.Scores)
                .HasForeignKey(s => s.PlayerId)
                .OnDelete(DeleteBehavior.Cascade); // If a player is deleted, remove their scores

            // ✅ Define Relationships for GameResult
            modelBuilder.Entity<GameResult>()
                .HasOne(gr => gr.Player1)
                .WithMany()
                .HasForeignKey(gr => gr.Player1Id)
                .OnDelete(DeleteBehavior.Restrict); // Prevent accidental cascading deletes

            modelBuilder.Entity<GameResult>()
                .HasOne(gr => gr.Player2)
                .WithMany()
                .HasForeignKey(gr => gr.Player2Id)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }
}
