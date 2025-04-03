using BananaGameAPI.Data;
using BananaGameAPI.DTOs;
using BananaGameAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BananaGameAPI.Services
{
    public class AuthService
    {
        private readonly GameDbContext _context;
        private readonly PasswordHasher<Player> _passwordHasher;

        // Injecting PasswordHasher using Dependency Injection
        public AuthService(GameDbContext context, PasswordHasher<Player> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        // Register a new player
        public async Task<string> RegisterPlayer(RegisterPlayerDto dto)
        {
            // Validate inputs
            if (string.IsNullOrEmpty(dto.Email) || string.IsNullOrEmpty(dto.Username) || string.IsNullOrEmpty(dto.Password))
                return "All fields are required!";

            // Check if email already exists
            if (await _context.Players.AnyAsync(p => p.Email == dto.Email))
                return "Email already exists!";

            // Create new player
            var newPlayer = new Player
            {
                Username = dto.Username,
                Email = dto.Email,
                // Hash the password before saving
                PasswordHash = _passwordHasher.HashPassword(null, dto.Password)
            };

            _context.Players.Add(newPlayer);
            await _context.SaveChangesAsync();

            return "Player registered successfully!";
        }

        // Login player
        public async Task<Player?> LoginPlayer(LoginPlayerDto dto)
        {
            // Find player by email
            var player = await _context.Players.FirstOrDefaultAsync(p => p.Email == dto.Email);
            if (player == null)
                return null;

            // Verify password
            var verificationResult = _passwordHasher.VerifyHashedPassword(player, player.PasswordHash, dto.Password);
            if (verificationResult == PasswordVerificationResult.Failed)
                return null;

            // Return player if login is successful
            return player;
        }
    }
}
