using FizzBuzzGameBackend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FizzBuzzGameBackend.Controllers
{
    [ApiController]
    [Route("api/game")]
    public class GameController : ControllerBase
    {
        private readonly AppDbContext _context;

        public GameController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("create")]
        public IActionResult CreateGame([FromBody] GameSession gameSession)
        {
            _context.GameSessions.Add(gameSession);
            _context.SaveChanges();
            return Ok(new { id = gameSession.Id });
        }

        [HttpGet("saved-games")]
        public IActionResult GetSavedGames()
        {
            var games = _context.GameSessions.Include(gs => gs.Rules).ToList();
            return Ok(games);
        }

        [HttpGet("rules/{sessionId}")]
        public IActionResult GetGameRules(int sessionId)
        {
            var rules = _context.GameRules.Where(r => r.GameSessionId == sessionId).ToList();
            return Ok(rules);
        }

        [HttpDelete("delete/{id}")]
        public IActionResult DeleteGame(int id)
        {
            var gameSession = _context.GameSessions.Find(id);
            if (gameSession == null) return NotFound();

            _context.GameSessions.Remove(gameSession);
            _context.SaveChanges();

            return Ok();
        }

        [HttpPut("edit/{id}")]
        public IActionResult EditGame(int id, [FromBody] GameSession gameSession)
        {
            var existingGameSession = _context.GameSessions.Find(id);
            if (existingGameSession == null) return NotFound();

            existingGameSession.GameName = gameSession.GameName;
            existingGameSession.Duration = gameSession.Duration;
            existingGameSession.Rules = gameSession.Rules;

            _context.SaveChanges();
            return Ok(existingGameSession);
        }

        [HttpGet("next")]
        public IActionResult GetNextNumber()
        {
            int number = new Random().Next(1, 101);
            return Ok(new { number });
        }

        [HttpPost("check")]
        public IActionResult CheckAnswer([FromBody] AnswerRequest request)
        {
            var gameRules = _context.GameRules.Where(r => r.GameSessionId == request.GameSessionId).ToList();
            string correctAnswer = GetCustomGameValue(request.Number, gameRules);
            bool isCorrect = request.Answer.Trim().ToLower() == correctAnswer.ToLower();
            return Ok(new { isCorrect, correctAnswer });
        }

        private string GetCustomGameValue(int number, List<GameRule> rules)
        {
            var result = "";
            foreach (var rule in rules)
            {
                if (number % rule.Divisor == 0)
                {
                    result += rule.ReplacementText;
                }
            }

            return string.IsNullOrEmpty(result) ? number.ToString() : result;
        }
    }
}