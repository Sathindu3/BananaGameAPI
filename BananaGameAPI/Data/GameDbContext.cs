using Microsoft.EntityFrameworkCore;
using BananaGameAPI.Models;

namespace BananaGameAPI.Data
{
    public class GameDbContext : DbContext
    {
        public GameDbContext(DbContextOptions<GameDbContext> options) : base(options) { }

        public DbSet<Player> Players { get; set; }
        public DbSet<Score> Scores { get; set; }
    }
}
