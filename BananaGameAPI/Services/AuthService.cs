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

        public AuthService(GameDbContext context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<Player>();
        }

        public async Task<string> RegisterPlayer(RegisterPlayerDto dto)
        {
            if (string.IsNullOrEmpty(dto.Email) || string.IsNullOrEmpty(dto.Username) || string.IsNullOrEmpty(dto.Password))
                return "All fields are required!";

            if (await _context.Players.AnyAsync(p => p.Email == dto.Email))
                return "Email already exists!";

            var newPlayer = new Player
            {
                Username = dto.Username,
                Email = dto.Email,
                PasswordHash = _passwordHasher.HashPassword(null, dto.Password)
            };

            _context.Players.Add(newPlayer);
            await _context.SaveChangesAsync();

            return "Player registered successfully!";
        }


        public async Task<Player?> LoginPlayer(LoginPlayerDto dto)
        {
            var player = await _context.Players.FirstOrDefaultAsync(p => p.Email == dto.Email);
            if (player == null)
                return null;

            var verificationResult = _passwordHasher.VerifyHashedPassword(player, player.PasswordHash, dto.Password);
            if (verificationResult == PasswordVerificationResult.Failed)
                return null;

            return player; // ✅ Return player details instead of just a string
        }

    }
}
