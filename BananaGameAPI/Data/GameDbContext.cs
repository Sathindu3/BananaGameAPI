using Microsoft.EntityFrameworkCore;
using BananaGameAPI.Models;

namespace BananaGameAPI.Data
{
    public class GameDbContext : DbContext
    {
        public GameDbContext(DbContextOptions<GameDbContext> options) : base(options) { }

        public DbSet<Player> Players { get; set; }
        public DbSet<Score> Scores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ✅ Ensure Username and Email are Unique
            modelBuilder.Entity<Player>()
                .HasIndex(p => p.Username)
                .IsUnique();

            modelBuilder.Entity<Player>()
                .HasIndex(p => p.Email)
                .IsUnique();

            // ✅ Define One-to-Many Relationship
            modelBuilder.Entity<Score>()
                .HasOne(s => s.Player)
                .WithMany(p => p.Scores) // ✅ A player has many scores
                .HasForeignKey(s => s.PlayerId)
                .OnDelete(DeleteBehavior.Cascade); // If a player is deleted, remove their scores

            base.OnModelCreating(modelBuilder);
        }
    }
}
