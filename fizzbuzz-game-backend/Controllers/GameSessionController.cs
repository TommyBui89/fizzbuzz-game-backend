using FizzBuzzGameBackend.Data;
using FizzBuzzGameBackend.DTOs;
using FizzBuzzGameBackend.Models;
using FizzBuzzGameBackend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FizzBuzzGameBackend.Controllers
{
    [ApiController]
    [Route("api")]
    public class GameSessionController : ControllerBase
    {
        private readonly FizzBuzzDbContext _context;
        private readonly GameSessionManager _sessionManager;

        public GameSessionController(FizzBuzzDbContext context, GameSessionManager sessionManager)
        {
            _context = context;
            _sessionManager = sessionManager;
        }

        // POST: api/games/{gameId}/sessions?durationSeconds=60
        [HttpPost("games/{gameId}/sessions")]
        public async Task<IActionResult> StartSession(int gameId, [FromQuery] int durationSeconds = 60)
        {
            var game = await _context.Games
                .Include(g => g.Rules)
                .FirstOrDefaultAsync(g => g.Id == gameId);
            if (game == null)
                return NotFound("Game not found.");

            // We are no longer using MaxNumber or a start time, but keep duration for the session
            var session = _sessionManager.CreateSession(gameId, durationSeconds);

            // Return session info and the first number
            return Ok(new
            {
                sessionId = session.SessionId,
                number = session.LastNumber,
                rules = game.Rules.Select(r => new { r.Divisor, r.ReplacementText })
            });
        }

        // POST: api/sessions/{sessionId}/submit
        [HttpPost("sessions/{sessionId}/submit")]
        public async Task<IActionResult> SubmitAnswer(Guid sessionId, [FromBody] SubmitAnswerRequestDto request)
        {
            if (!_sessionManager.TryGetSession(sessionId, out var session))
                return NotFound("Session not found.");

            // We removed the date/time check, so no expiry logic here

            var game = await _context.Games
                .Include(g => g.Rules)
                .FirstOrDefaultAsync(g => g.Id == session.GameId);
            if (game == null)
                return NotFound("Game not found.");

            // Validate answer
            var correctAnswer = FizzBuzzHelper.ComputeResult(session.LastNumber.Value, game.Rules);
            bool isCorrect = string.Equals(correctAnswer, request.Answer, StringComparison.OrdinalIgnoreCase);

            if (isCorrect)
                session.CorrectCount++;
            else
                session.IncorrectCount++;

            // Generate next random number from the fixed range
            int? nextNumber = null;
            try
            {
                nextNumber = _sessionManager.GetNextRandomNumber(session);
            }
            catch (Exception)
            {
                // All numbers used
                nextNumber = null;
            }

            return Ok(new
            {
                correct = isCorrect,
                correctAnswer,
                score = new { session.CorrectCount, session.IncorrectCount },
                nextNumber
            });
        }
    }
}