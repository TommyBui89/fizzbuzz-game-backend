namespace FizzBuzzGameBackend.Models
{
    public class Game
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }

        public List<Rule> Rules { get; set; } = new();
    }
}