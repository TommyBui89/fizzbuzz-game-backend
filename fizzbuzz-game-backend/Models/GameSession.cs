using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FizzBuzzGameBackend.Models
{
    public class GameSession
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string GameName { get; set; } = string.Empty;

        [Required]
        public string Author { get; set; } = string.Empty;

        public List<GameRule> Rules { get; set; } = new();
    }
}