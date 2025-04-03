using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace BananaGameAPI.Models
{
    public class Player
    {
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; } // ✅ Ensure this exists for authentication

        // ✅ Add a navigation property for Scores
        public ICollection<Score> Scores { get; set; } = new List<Score>();

        // ✅ Add a navigation property for GameResults
        public ICollection<GameResult> GameResults { get; set; } = new List<GameResult>();
    }

    public class Score
    {
        public int Id { get; set; }

        [Required]
        public int PlayerId { get; set; }  // ✅ Foreign Key

        [ForeignKey("PlayerId")]
        public Player Player { get; set; } // ✅ Navigation Property

        [Required]
        public int Points { get; set; }

        public DateTime DateRecorded { get; set; } = DateTime.UtcNow;
    }

    public class GameResult
    {
        public int Id { get; set; }

        [Required]
        public int Player1Id { get; set; }

        [ForeignKey("Player1Id")]
        public Player Player1 { get; set; } // ✅ Navigation Property

        [Required]
        public int Player2Id { get; set; }

        [ForeignKey("Player2Id")]
        public Player Player2 { get; set; } // ✅ Navigation Property

        [Required]
        public int Player1Score { get; set; }

        [Required]
        public int Player2Score { get; set; }

        [Required]
        public string Winner { get; set; } // Stores Username of Winner

        public DateTime GameDate { get; set; } = DateTime.UtcNow;
    }

    public class QuizResponse
    {
        [JsonProperty("question")]
        public string Question { get; set; } = string.Empty;

        [JsonProperty("solution")]
        public string Solution { get; set; } = string.Empty;
    }
}
