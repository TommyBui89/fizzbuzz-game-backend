using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FizzBuzzGameBackend.Models
{
    public class GameSession
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string GameName { get; set; }
        public int Duration { get; set; }
        [Required]
        public ICollection<GameRule> Rules { get; set; } = new List<GameRule>();
    }
}