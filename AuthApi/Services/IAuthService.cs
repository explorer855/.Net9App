using AuthApi.Models.Dtos;

namespace AuthApi.Services
{
    public interface IAuthService
    {
        /// <summary>
        /// Registers a new user with the provided email and password.
        /// </summary>
        /// <param name="registerUser">The user registration model.</param>
        /// <returns>A task that represents the asynchronous operation, containing the result of the registration.</returns>
        Task<string?> RegisterAsync(RegisterUserRequest registerUser);
        
        /// <summary>
        /// Logs in a user with the provided email and password.
        /// </summary>
        /// <param name="userLogin">The user login model.</param>
        /// <returns>A task that represents the asynchronous operation, containing the User-Id and IsValid Login flag.</returns>
        Task<(string?, bool)> LoginAsync(UserLoginRequest userLogin);
    }
}
