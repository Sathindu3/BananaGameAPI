using Microsoft.AspNetCore.Mvc;
using BananaGameAPI.Data;
using BananaGameAPI.Models;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

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

        // Fetch Quiz
        [HttpGet("quiz")]
        public async Task<IActionResult> GetQuiz()
        {
            string apiUrl = "https://marcconrad.com/uob/banana/api.php";
            var response = await _httpClient.GetStringAsync(apiUrl);
            var quiz = JsonConvert.DeserializeObject<QuizResponse>(response);
            return Ok(quiz);
        }

        // Submit Score
        [HttpPost("score")]
        public IActionResult SubmitScore([FromBody] Score score)
        {
            _context.Scores.Add(score);
            _context.SaveChanges();
            return Ok(new { message = "Score saved!" });
        }

        // Get Leaderboard
        [HttpGet("leaderboard")]
        public IActionResult GetLeaderboard()
        {
            var leaderboard = _context.Scores.OrderByDescending(s => s.Points).Take(5).ToList();
            return Ok(leaderboard);
        }
    }
}
