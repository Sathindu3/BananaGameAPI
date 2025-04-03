using BananaGameAPI.DTOs;
using BananaGameAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BananaGameAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

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

        // ✅ New GET Login Method
        [HttpPost("login")] // Change from [HttpGet] to [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginPlayerDto dto) // Accept JSON body
        {
            var player = await _authService.LoginPlayer(dto);

            if (player == null)
                return Unauthorized(new { message = "Invalid email or password!" });

            return Ok(new
            {
                id = player.Id,
                username = player.Username,
                email = player.Email
            });
        }


    }
}
