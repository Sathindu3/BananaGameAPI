using BananaGameAPI.DTOs;
using BananaGameAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BananaGameAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        // Injecting AuthService to interact with the authentication logic
        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        // Register a new player
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterPlayerDto dto)
        {
            if (dto == null)
                return BadRequest(new { message = "Invalid request body." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.RegisterPlayer(dto);
            if (result == "Email already exists!")
                return Conflict(new { message = result });

            return Ok(new { message = result });
        }

        // Login player and store session data
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginPlayerDto dto)
        {
            // Attempt to log in the player and set session data
            var player = await _authService.LoginPlayer(dto, HttpContext.Session);

            if (player == null)
                return Unauthorized(new { message = "Invalid email or password!" });

            // Respond with player details after successful login
            return Ok(new
            {
                id = player.Id,
                username = player.Username,
                email = player.Email
            });
        }

        // Logout player and clear session
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            // Clear session when logging out
            _authService.LogoutPlayer(HttpContext.Session);
            return Ok(new { message = "Logged out successfully!" });
        }

        // Check if the user is logged in
        [HttpGet("check-login")]
        public IActionResult CheckLogin()
        {
            // Check session for login status
            var isLoggedIn = _authService.IsLoggedIn(HttpContext.Session);
            return Ok(new { isLoggedIn });
        }
    }
}
