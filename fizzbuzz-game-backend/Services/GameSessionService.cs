using FizzBuzzGameBackend.Data;
using FizzBuzzGameBackend.DTOs;
using FizzBuzzGameBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace FizzBuzzGameBackend.Services
{
    public class GameSessionService : IGameSessionService
    {
        private readonly FizzBuzzDbContext _context;
        private readonly GameSessionManager _sessionManager;

        public GameSessionService(FizzBuzzDbContext context, GameSessionManager sessionManager)
        {
            _context = context;
            _sessionManager = sessionManager;
        }

        public async Task<object?> StartSessionAsync(int gameId, int durationSeconds)
        {
            var game = await _context.Games.Include(g => g.Rules).FirstOrDefaultAsync(g => g.Id == gameId);
            if (game == null) return null;

            var session = _sessionManager.CreateSession(gameId, durationSeconds);

            return new
            {
                sessionId = session.SessionId,
                number = session.LastNumber,
                rules = game.Rules.Select(r => new { r.Divisor, r.ReplacementText })
            };
        }

        public async Task<object?> SubmitAnswerAsync(Guid sessionId, SubmitAnswerRequestDto dto)
        {
            if (!_sessionManager.TryGetSession(sessionId, out var session))
                return null;

            var game = await _context.Games.Include(g => g.Rules).FirstOrDefaultAsync(g => g.Id == session.GameId);
            if (game == null) return null;

            var correctAnswer = FizzBuzzHelper.ComputeResult(session.LastNumber.Value, game.Rules);
            bool isCorrect = string.Equals(correctAnswer, dto.Answer, StringComparison.OrdinalIgnoreCase);

            if (isCorrect)
                session.CorrectCount++;
            else
                session.IncorrectCount++;

            int? nextNumber = null;
            try
            {
                nextNumber = _sessionManager.GetNextRandomNumber(session);
            }
            catch (Exception)
            {
                nextNumber = null;
            }

            return new
            {
                correct = isCorrect,
                correctAnswer,
                score = new { session.CorrectCount, session.IncorrectCount },
                nextNumber
            };
        }
    }
}