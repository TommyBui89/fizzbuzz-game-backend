namespace FizzBuzzGameBackend.Models
{
    public class AnswerRequest
    {
        public int GameSessionId { get; set; }
        public int Number { get; set; }
        public string Answer { get; set; }
    }
}