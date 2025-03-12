namespace FizzBuzzGameBackend.DTOs
{
    public class CreateGameDto
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public List<CreateRuleDto> Rules { get; set; } = new();
    }

    public class CreateRuleDto
    {
        public int Divisor { get; set; }
        public string ReplacementText { get; set; }
    }
}