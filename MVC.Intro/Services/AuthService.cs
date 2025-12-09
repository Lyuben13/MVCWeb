using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using MVC.Intro.Data;
using MVC.Intro.Models;
using MVC.Intro.ViewModels;

namespace MVC.Intro.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<AuthService> _logger;

        public AuthService(AppDbContext context, ILogger<AuthService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<User?> RegisterAsync(RegisterViewModel model)
        {
            try
            {
                if (await EmailExistsAsync(model.Email))
                {
                    _logger.LogWarning("Attempt to register with existing email: {Email}", model.Email);
                    return null;
                }

                var user = new User
                {
                    Id = Guid.NewGuid(),
                    Name = model.Name,
                    Email = model.Email.ToLowerInvariant(),
                    PasswordHash = HashPassword(model.Password),
                    CreatedAt = DateTime.UtcNow
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                _logger.LogInformation("User registered successfully: {Email}", user.Email);
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error registering user: {Email}", model.Email);
                return null;
            }
        }

        public async Task<User?> LoginAsync(string email, string password)
        {
            try
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLowerInvariant());

                if (user == null)
                {
                    _logger.LogWarning("Login attempt with non-existent email: {Email}", email);
                    return null;
                }

                if (!VerifyPassword(password, user.PasswordHash))
                {
                    _logger.LogWarning("Invalid password attempt for email: {Email}", email);
                    return null;
                }

                user.LastLoginAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();

                _logger.LogInformation("User logged in successfully: {Email}", email);
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login for email: {Email}", email);
                return null;
            }
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Users
                .AnyAsync(u => u.Email.ToLower() == email.ToLowerInvariant());
        }

        private static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

        private static bool VerifyPassword(string password, string hash)
        {
            var passwordHash = HashPassword(password);
            return passwordHash == hash;
        }
    }
}