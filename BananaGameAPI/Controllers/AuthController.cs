using Microsoft.AspNetCore.Mvc;
using BananaGameAPI.Data;
using BananaGameAPI.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BananaGameAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly GameDbContext _context;

        public AuthController(GameDbContext context)
        {
            _context = context;
        }

        // Player Signup
        [HttpPost("signup")]
        public async Task<IActionResult> Signup([FromBody] Player player)
        {
            if (_context.Players.Any(p => p.Username == player.Username))
            {
                return BadRequest(new { message = "Username already exists" });
            }

            _context.Players.Add(player);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Signup successful!" });
        }

        // Player Login
        [HttpPost("login")]
        public IActionResult Login([FromBody] Player player)
        {
            var existingPlayer = _context.Players.FirstOrDefault(p => p.Username == player.Username && p.Password == player.Password);
            if (existingPlayer == null)
            {
                return Unauthorized(new { message = "Invalid credentials" });
            }

            return Ok(new { message = "Login successful!", playerId = existingPlayer.Id, username = existingPlayer.Username });
        }
    }
}
