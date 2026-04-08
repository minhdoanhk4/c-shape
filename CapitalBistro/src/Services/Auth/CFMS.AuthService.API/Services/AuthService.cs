using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using CFMS.AuthService.Core.Entities;
using CFMS.AuthService.Core.Interfaces;

namespace CFMS.AuthService.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IConfiguration _configuration;

        public AuthService(IUserRepository userRepository, IPasswordHasher passwordHasher, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _configuration = configuration;
        }

        public async Task<(bool Success, string Token, string Message)> LoginAsync(string username, string password)
        {
            var user = await _userRepository.GetUserByUsernameAsync(username);
            
            if (user == null)
            {
                return (false, string.Empty, "Invalid username or password.");
            }

            if (user.IsLockedOut && user.LockoutEnd > DateTimeOffset.UtcNow)
            {
                return (false, string.Empty, $"Account is locked. Try again after {user.LockoutEnd.Value.ToLocalTime()}");
            }

            if (!_passwordHasher.VerifyPassword(password, user.PasswordHash))
            {
                user.FailedLoginAttempts++;
                if (user.FailedLoginAttempts >= 5)
                {
                    user.IsLockedOut = true;
                    user.LockoutEnd = DateTimeOffset.UtcNow.AddMinutes(30); // 30 minutes lockout
                }
                await _userRepository.UpdateUserAsync(user);
                await _userRepository.SaveChangesAsync();
                
                return (false, string.Empty, "Invalid username or password.");
            }

            // Reset login attempts on success
            user.FailedLoginAttempts = 0;
            user.IsLockedOut = false;
            user.LockoutEnd = null;
            await _userRepository.UpdateUserAsync(user);
            await _userRepository.SaveChangesAsync();

            var token = GenerateJwtToken(user);
            return (true, token, "Login successful");
        }

        public async Task<(bool Success, string Message)> RegisterAsync(string username, string password, string roleName, Guid? franchiseId)
        {
            if (await _userRepository.ExistsByUsernameAsync(username))
            {
                return (false, "Username already exists.");
            }

            var role = await _userRepository.GetRoleByNameAsync(roleName);
            if (role == null)
            {
                return (false, "Invalid role.");
            }

            var user = new User
            {
                Username = username,
                PasswordHash = _passwordHasher.HashPassword(password),
                RoleId = role.Id,
                FranchiseId = franchiseId,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            };

            await _userRepository.AddUserAsync(user);
            await _userRepository.SaveChangesAsync();

            return (true, "User registered successfully");
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role?.Name ?? "User"),
                new Claim("FranchiseId", user.FranchiseId?.ToString() ?? string.Empty)
            };

            var secretKey = _configuration["JwtSettings:SecretKey"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(int.Parse(_configuration["JwtSettings:ExpirationMinutes"] ?? "1440")), // 1 day default
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
