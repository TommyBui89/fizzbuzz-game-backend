using System.ComponentModel.DataAnnotations;

namespace FizzBuzzGameBackend.Models
{
    public class GameSession
    {
        [Key]
        public Guid SessionId { get; set; }

        public int GameId { get; set; }
        public int DurationSeconds { get; set; }
        public HashSet<int> NumbersUsed { get; set; } = new();
        public int CorrectCount { get; set; }
        public int IncorrectCount { get; set; }
        public int? LastNumber { get; set; }
    }
}
