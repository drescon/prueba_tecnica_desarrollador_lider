
using GestionSolicitudes.Application.Dto.Request;
using GestionSolicitudes.Application.Dto.Response;
using GestionSolicitudes.Application.Auth;
using Microsoft.AspNetCore.Mvc;

namespace GestionSolicitudes.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            LoginResponse result = await _authService.LoginAsync(request);
            return Ok(result);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            RegisterResponse result = await _authService.RegisterAsync(request);
            return CreatedAtAction(nameof(Register), new { userId = result.UserId }, result);
        }
    }
}