using FizzBuzzGameBackend.Data;
using FizzBuzzGameBackend.DTOs;
using FizzBuzzGameBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace FizzBuzzGameBackend.Services
{
    public class GameService : IGameService
    {
        private readonly FizzBuzzDbContext _context;

        public GameService(FizzBuzzDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<GameResponseDto>> GetAllGamesAsync()
        {
            var games = await _context.Games.Include(g => g.Rules).ToListAsync();
            return games.Select(MapToDto);
        }

        public async Task<GameResponseDto?> GetGameByIdAsync(int id)
        {
            var game = await _context.Games.Include(g => g.Rules).FirstOrDefaultAsync(g => g.Id == id);
            return game == null ? null : MapToDto(game);
        }

        public async Task<GameResponseDto> CreateGameAsync(CreateGameDto dto)
        {
            var game = new Game
            {
                Name = dto.Name,
                Author = dto.Author,
                Rules = dto.Rules.Select(r => new Rule
                {
                    Divisor = r.Divisor,
                    ReplacementText = r.ReplacementText
                }).ToList()
            };

            _context.Games.Add(game);
            await _context.SaveChangesAsync();
            return MapToDto(game);
        }

        public async Task<bool> UpdateGameAsync(UpdateGameDto dto)
        {
            var game = await _context.Games.Include(g => g.Rules).FirstOrDefaultAsync(g => g.Id == dto.Id);
            if (game == null) return false;

            game.Name = dto.Name;
            game.Author = dto.Author;

            _context.Rules.RemoveRange(game.Rules);
            game.Rules = dto.Rules.Select(r => new Rule
            {
                Divisor = r.Divisor,
                ReplacementText = r.ReplacementText
            }).ToList();

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteGameAsync(int id)
        {
            var game = await _context.Games.FindAsync(id);
            if (game == null) return false;

            _context.Games.Remove(game);
            await _context.SaveChangesAsync();
            return true;
        }

        private static GameResponseDto MapToDto(Game game)
        {
            return new GameResponseDto
            {
                Id = game.Id,
                Name = game.Name,
                Author = game.Author,
                Rules = game.Rules.Select(r => new RuleDto
                {
                    Divisor = r.Divisor,
                    ReplacementText = r.ReplacementText
                }).ToList()
            };
        }
    }
}
