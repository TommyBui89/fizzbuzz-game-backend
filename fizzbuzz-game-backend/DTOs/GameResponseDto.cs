namespace FizzBuzzGameBackend.DTOs
{
    public class GameResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public List<RuleDto> Rules { get; set; } = new();
    }

    public class RuleDto
    {
        public int Divisor { get; set; }
        public string ReplacementText { get; set; }
    }
}