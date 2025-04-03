using Microsoft.AspNetCore.Mvc;
using BananaGameAPI.Data;
using BananaGameAPI.Models;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace BananaGameAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly GameDbContext _context;
        private readonly HttpClient _httpClient;

        public GameController(GameDbContext context)
        {
            _context = context;
            _httpClient = new HttpClient();
        }

        // ✅ Fetch Quiz from External API
        [HttpGet("quiz")]
        public async Task<IActionResult> GetQuiz()
        {
            try
            {
                string apiUrl = "https://marcconrad.com/uob/banana/api.php";
                var response = await _httpClient.GetStringAsync(apiUrl);

                // ✅ Log API Response (for debugging)
                Console.WriteLine("RAW API Response: " + response);

                var quiz = JsonConvert.DeserializeObject<QuizResponse>(response);

                if (quiz == null || string.IsNullOrEmpty(quiz.Question))
                {
                    return NotFound(new { success = false, message = "Quiz data not found" });
                }

                return Ok(new { success = true, data = quiz });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Error fetching quiz", error = ex.Message });
            }
        }

        // ✅ Submit Score (for individual player actions)
        [HttpPost("score")]
        public async Task<IActionResult> SubmitScore([FromBody] Score score)
        {
            if (score == null || score.PlayerId <= 0)
            {
                return BadRequest(new { success = false, message = "Invalid score data" });
            }

            try
            {
                var player = await _context.Players.FindAsync(score.PlayerId);
                if (player == null)
                {
                    return NotFound(new { success = false, message = "Player not found!" });
                }

                // Create and save the new score
                var newScore = new Score
                {
                    PlayerId = player.Id,
                    Points = score.Points
                };

                _context.Scores.Add(newScore);
                await _context.SaveChangesAsync();

                return Ok(new { success = true, message = "Score saved!", data = newScore });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Error saving score", error = ex.Message });
            }
        }

        // ✅ Get Top 5 Leaderboard Scores
        [HttpGet("leaderboard")]
        public IActionResult GetLeaderboard()
        {
            try
            {
                var leaderboard = _context.Scores
                    .OrderByDescending(s => s.Points)
                    .Take(5)
                    .ToList();

                return Ok(new { success = true, data = leaderboard });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Error retrieving leaderboard", error = ex.Message });
            }
        }

        // ✅ Save Game Result (this includes winner information and match result)
        
    }
}
