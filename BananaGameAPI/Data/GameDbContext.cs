using Microsoft.EntityFrameworkCore;
using BananaGameAPI.Models;

namespace BananaGameAPI.Data
{
    public class GameDbContext : DbContext
    {
        public GameDbContext(DbContextOptions<GameDbContext> options) : base(options) { }

        public DbSet<Player> Players { get; set; }
        public DbSet<Score> Scores { get; set; }
        public DbSet<GameResult> GameResults { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
            modelBuilder.Entity<Player>()
                .HasIndex(p => p.Username)
                .IsUnique();

            modelBuilder.Entity<Player>()
                .HasIndex(p => p.Email)
                .IsUnique();

            
            modelBuilder.Entity<Score>()
                .HasOne(s => s.Player)
                .WithMany(p => p.Scores)
                .HasForeignKey(s => s.PlayerId)
                .OnDelete(DeleteBehavior.Cascade); 

          
            modelBuilder.Entity<GameResult>()
                .HasOne(gr => gr.Player1)
                .WithMany()
                .HasForeignKey(gr => gr.Player1Id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<GameResult>()
                .HasOne(gr => gr.Player2)
                .WithMany()
                .HasForeignKey(gr => gr.Player2Id)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }
}
