using Microsoft.AspNetCore.Mvc;

namespace FizzBuzzGameBackend.Controllers
{
    [ApiController]
    [Route("api/game")]
    public class GameController : ControllerBase
    {
        private static readonly Random _random = new();

        [HttpGet("next")]
        public IActionResult GetNextNumber()
        {
            int number = _random.Next(1, 101);
            return Ok(new { number });
        }

        [HttpPost("check")]
        public IActionResult CheckAnswer([FromBody] AnswerRequest request)
        {
            string correctAnswer = GetFizzBuzzValue(request.Number);
            bool isCorrect = request.Answer.Trim().ToLower() == correctAnswer.ToLower();
            return Ok(new { isCorrect, correctAnswer });
        }

        private static string GetFizzBuzzValue(int number)
        {
            if (number % 15 == 0) return "FizzBuzz";
            if (number % 3 == 0) return "Fizz";
            if (number % 5 == 0) return "Buzz";
            return number.ToString();
        }
    }

    public class AnswerRequest
    {
        public int Number { get; set; }
        public string Answer { get; set; } = string.Empty;
    }
}