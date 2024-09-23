using Microsoft.AspNetCore.Mvc;
using CSMessagingApp.Server.Services;
using CSMessagingApp.Shared.DTOs;

namespace CSMessagingApp.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var token = await _authService.AuthenticateAsync(loginDto.Username, loginDto.Password);
            if (token == null)
                return Unauthorized();
            return Ok(new { Token = token });
        }
    }
}
