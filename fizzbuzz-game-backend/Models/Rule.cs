namespace FizzBuzzGameBackend.Models
{
    public class Rule
    {
        public int Id { get; set; }
        public int Divisor { get; set; }
        public string ReplacementText { get; set; }

        // Relationship
        public int GameId { get; set; }
        public Game Game { get; set; }
    }
}