using FizzBuzzGameBackend.Data;
using FizzBuzzGameBackend.DTOs;
using FizzBuzzGameBackend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace FizzBuzzGameBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameController : ControllerBase
    {
        private readonly FizzBuzzDbContext _context;

        public GameController(FizzBuzzDbContext context)
        {
            _context = context;
        }

        // GET: api/game
        [HttpGet]
        public async Task<IActionResult> GetAllGames()
        {
            var games = await _context.Games
                .Include(g => g.Rules)
                .ToListAsync();

            var response = games.Select(game => new GameResponseDto
            {
                Id = game.Id,
                Name = game.Name,
                Author = game.Author,
                Rules = game.Rules.Select(r => new RuleDto
                {
                    Divisor = r.Divisor,
                    ReplacementText = r.ReplacementText
                }).ToList()
            });

            return Ok(response);
        }

        // GET: api/game/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetGameById(int id)
        {
            var game = await _context.Games
                .Include(g => g.Rules)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (game == null)
                return NotFound();

            var response = new GameResponseDto
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

            return Ok(response);
        }

        // POST: api/game
        [HttpPost]
        public async Task<IActionResult> CreateGame([FromBody] CreateGameDto dto)
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

            var response = new GameResponseDto
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

            return CreatedAtAction(nameof(GetGameById), new { id = game.Id }, response);
        }

        // PUT: api/game/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGame(int id, [FromBody] UpdateGameDto dto)
        {
            if (id != dto.Id)
                return BadRequest("ID mismatch.");

            var game = await _context.Games
                .Include(g => g.Rules)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (game == null)
                return NotFound();

            game.Name = dto.Name;
            game.Author = dto.Author;

            _context.Rules.RemoveRange(game.Rules);
            game.Rules = dto.Rules.Select(r => new Rule
            {
                Divisor = r.Divisor,
                ReplacementText = r.ReplacementText
            }).ToList();

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/game/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGame(int id)
        {
            var game = await _context.Games.FindAsync(id);
            if (game == null)
                return NotFound();

            _context.Games.Remove(game);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
