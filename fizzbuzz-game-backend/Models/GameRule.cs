using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FizzBuzzGameBackend.Models
{
    public class GameRule
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int Divisor { get; set; }

        [Required]
        public string ReplacementText { get; set; } = string.Empty;

        public int GameSessionId { get; set; }
    }
}