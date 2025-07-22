using AuthApi.Data;
using AuthApi.Models.Dtos;
using AuthApi.Models.Entities;
using Microsoft.AspNetCore.Identity;

namespace AuthApi.Infrastructure.Services
{
    /// <summary>
    /// Class for handling authentication services.
    /// </summary>
    public class AuthService(UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager)
        : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));

        /// <summary>
        /// Login a user with the provided credentials.
        /// </summary>
        /// <param name="userLogin"></param>
        /// <returns></returns>
        public async Task<(string?, bool)> LoginAsync(UserLoginRequest userLogin)
        {
            try
            {
                var appUser = await _userManager.FindByEmailAsync(userLogin.Email);
                var siginIn = await _signInManager.PasswordSignInAsync(appUser.UserName, userLogin.Password, false, true);

                if (siginIn.Succeeded)
                {
                    return (appUser.Id, true);
                }
                
                return ("Incorrect Username/Password!", false);
            }
            catch(Exception ex)
            {
                throw new InvalidOperationException("An error occurred while registering the user.", ex);
            }
        }

        /// <summary>
        /// Login a user with Username in the provided credentials.
        /// </summary>
        /// <param name="userLogin"></param>
        /// <returns></returns>
        public async Task<(string?, bool)> LoginByUserNameAsync(UserNameLoginRequest userLogin)
        {
            try
            {
                var appUser = await _userManager.FindByNameAsync(userLogin.Username);
                var siginIn = await _signInManager.PasswordSignInAsync(appUser.UserName, userLogin.Password, false, true);

                if (siginIn.Succeeded)
                {
                    return (appUser.Id, true);
                }

                return ("Incorrect Username/Password!", false);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while registering the user.", ex);
            }
        }

        /// <summary>
        /// Register a new user with the provided details.
        /// </summary>
        /// <param name="registerUser"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<string?> RegisterAsync(RegisterUserRequest registerUser)
        {
            try
            {
               var response = await _userManager.CreateAsync(new ApplicationUser
                {
                    UserName = registerUser.Username,
                    Email = registerUser.Email,
                    PhoneNumber = registerUser.PhoneNumber,
                    Name = registerUser.Username // Assuming Name is the same as Username for simplicity
                }, registerUser.Password);

                return response.Succeeded 
                    ? "User registered successfully" 
                    : string.Join(", ", response.Errors.Select(e => e.Description));    
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                throw new InvalidOperationException("An error occurred while registering the user.", ex);
            }
        }
    }
}
