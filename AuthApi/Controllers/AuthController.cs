using AuthApi.Models.Dtos;
using AuthApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace AuthApi.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest user)
        {
            if (user is null)
            {
                return BadRequest("Login details cannot be null.");
            }

            var response = await _authService.LoginAsync(user);
            return Ok(response);
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest registerUser)
        {
            if (registerUser == null)
            {
                return BadRequest("Invalid user data.");
            }
            var result = await _authService.RegisterAsync(registerUser);
            return Ok(result);
        }
    }

}