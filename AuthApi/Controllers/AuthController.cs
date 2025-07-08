using AuthApi.Data.Models;
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

        [HttpGet]
        public async Task<IActionResult> Login([FromRoute]UserLoginModel user)
        {
            return Ok();
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserModel registerUser)
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