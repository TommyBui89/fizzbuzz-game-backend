using FizzBuzzGameBackend.DTOs;
using FizzBuzzGameBackend.Services;
using Microsoft.AspNetCore.Mvc;

namespace FizzBuzzGameBackend.Controllers
{
    [ApiController]
    [Route("api")]
    public class GameSessionController : ControllerBase
    {
        private readonly IGameSessionService _sessionService;

        public GameSessionController(IGameSessionService sessionService)
        {
            _sessionService = sessionService;
        }

        [HttpPost("games/{gameId}/sessions")]
        public async Task<IActionResult> StartSession(int gameId, [FromQuery] int durationSeconds = 60)
        {
            var result = await _sessionService.StartSessionAsync(gameId, durationSeconds);
            return result == null ? NotFound("Game not found.") : Ok(result);
        }

        [HttpPost("sessions/{sessionId}/submit")]
        public async Task<IActionResult> SubmitAnswer(Guid sessionId, [FromBody] SubmitAnswerRequestDto dto)
        {
            var result = await _sessionService.SubmitAnswerAsync(sessionId, dto);
            return result == null ? NotFound("Session or game not found.") : Ok(result);
        }
    }
}