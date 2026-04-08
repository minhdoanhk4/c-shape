using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CFMS.AuthService.API.DTOs;
using CFMS.AuthService.Core.Interfaces;

namespace CFMS.AuthService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await _authService.LoginAsync(request.Username, request.Password);

            if (!result.Success)
            {
                return Unauthorized(new { message = result.Message });
            }

            return Ok(new AuthResponse { Token = result.Token, Message = result.Message });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            // Note: In real app, you might want to protect this endpoint with [Authorize(Roles = "Admin")]
            var result = await _authService.RegisterAsync(request.Username, request.Password, request.Role, request.FranchiseId);

            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }

            return Ok(new { message = result.Message });
        }
    }
}
