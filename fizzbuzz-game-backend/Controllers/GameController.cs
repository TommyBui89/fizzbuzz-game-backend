using FizzBuzzGameBackend.DTOs;
using FizzBuzzGameBackend.Services;
using Microsoft.AspNetCore.Mvc;

namespace FizzBuzzGameBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameController : ControllerBase
    {
        private readonly IGameService _gameService;

        public GameController(IGameService gameService)
        {
            _gameService = gameService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllGames()
        {
            var games = await _gameService.GetAllGamesAsync();
            return Ok(games);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetGameById(int id)
        {
            var game = await _gameService.GetGameByIdAsync(id);
            return game == null ? NotFound() : Ok(game);
        }

        [HttpPost]
        public async Task<IActionResult> CreateGame([FromBody] CreateGameDto dto)
        {
            var created = await _gameService.CreateGameAsync(dto);
            return CreatedAtAction(nameof(GetGameById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGame(int id, [FromBody] UpdateGameDto dto)
        {
            if (id != dto.Id) return BadRequest("ID mismatch.");
            var updated = await _gameService.UpdateGameAsync(dto);
            return updated ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGame(int id)
        {
            var deleted = await _gameService.DeleteGameAsync(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}