using IdentityApi.Infrastructure.Services;
using IdentityApi.Models.Dtos;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace IdentityApi.Controllers
{
    /// <summary>
    /// Provides authentication-related endpoints for user login and registration.
    /// </summary>
    /// <remarks>This controller handles user authentication operations, including login and registration. It
    /// interacts with the <see cref="IAuthService"/> to perform the necessary business logic.</remarks>
    /// <param name="authService"></param>


    [ApiController]
    [Route("api/auth")]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        private readonly IAuthService _authService = authService ?? throw new ArgumentNullException(nameof(authService));

        /// <summary>
        /// Endpoint for user login
        /// </summary>
        /// <param name="user">User Login Request</param>
        /// <returns></returns>
        [HttpPost("login/email")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(AuthResponseDto<>))]
        [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
        [ProducesResponseType(statusCode: StatusCodes.Status500InternalServerError, type: typeof(ProblemDetails))]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest user, IValidator<UserLoginRequest> validator)
        {
            var validations = await validator.ValidateAsync(user);

            if(!validations.IsValid)
            {
                throw new ValidationException(validations.IsValid.ToString(), validations.Errors);
            }

            var response = await _authService.LoginAsync(user);
            return Ok(new AuthResponseDto<string> { TResponse = response.Item1, IsSuccess = response.Item2 });
        }

        /// <summary>
        /// Endpoint for user login
        /// </summary>
        /// <param name="user">User Login Request</param>
        /// <returns></returns>
        [HttpPost("login/userName")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(AuthResponseDto<>))]
        [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
        [ProducesResponseType(statusCode: StatusCodes.Status500InternalServerError, type: typeof(ProblemDetails))]
        public async Task<IActionResult> LoginByUserName([FromBody] UserNameLoginRequest user, IValidator<UserNameLoginRequest> validator)
        {
            var validations = await validator.ValidateAsync(user);

            if (!validations.IsValid)
            {
                throw new ValidationException(validations.IsValid.ToString(), validations.Errors);
            }

            var response = await _authService.LoginByUserNameAsync(user);
            return Ok(new AuthResponseDto<string> { TResponse = response.Item1, IsSuccess = response.Item2 });
        }

        /// <summary>
        /// Endpoint for user registration
        /// </summary>
        /// <param name="registerUser">Register User Input Model</param>
        /// <returns>User Registration Status</returns>
        [HttpPost("register")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(AuthResponseDto<>))]
        [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
        [ProducesResponseType(statusCode: StatusCodes.Status500InternalServerError, type: typeof(ProblemDetails))]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest registerUser)
        {
            if (registerUser == null)
            {
                throw new ArgumentNullException(nameof(registerUser));
            }
            var result = await _authService.RegisterAsync(registerUser);
            return Ok(new AuthResponseDto<string>{ TResponse = result, IsSuccess = true });
        }
    }

}