using System.ComponentModel.DataAnnotations;

namespace FizzBuzzGameBackend.Models
{
    public class GameRule
    {
        [Key]
        public int Id { get; set; }
        public int Divisor { get; set; }
        public string ReplacementText { get; set; }
        public int GameSessionId { get; set; }
        public GameSession GameSession { get; set; }
    }
}