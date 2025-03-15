using System.ComponentModel.DataAnnotations;

namespace BananaGameAPI.Models
{
    public class Player
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }

    public class Score
    {
        [Key]
        public int Id { get; set; }
        public string PlayerName { get; set; } = string.Empty;
        public int Points { get; set; }
    }

    public class QuizResponse
    {
        public string Question { get; set; } = string.Empty;
        public string Answer { get; set; } = string.Empty;
    }
}
