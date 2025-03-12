namespace FizzBuzzGameBackend.DTOs
{
    public class UpdateGameDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public List<CreateRuleDto> Rules { get; set; } = new();
    }
}