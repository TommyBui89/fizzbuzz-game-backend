using FizzBuzzGameBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace FizzBuzzGameBackend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<GameSession> GameSessions { get; set; }
        public DbSet<GameRule> GameRules { get; set; }
    }
}