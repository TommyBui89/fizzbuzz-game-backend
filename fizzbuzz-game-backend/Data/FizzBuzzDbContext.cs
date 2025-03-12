using FizzBuzzGameBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace FizzBuzzGameBackend.Data
{
    public class FizzBuzzDbContext : DbContext
    {
        public FizzBuzzDbContext(DbContextOptions<FizzBuzzDbContext> options)
            : base(options)
        {
        }

        public DbSet<Game> Games { get; set; }
        public DbSet<Rule> Rules { get; set; }
        public DbSet<GameSession> GameSessions { get; set; }
    }
}