using BananaGameAPI.DTOs;
using BananaGameAPI.Services;
using Microsoft.AspNetCore.Mvc;

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
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.RegisterPlayer(dto);
            if (result == "Email already exists!")
                return Conflict(new { message = result });

            return Ok(new { message = result });
        }
    }
}
