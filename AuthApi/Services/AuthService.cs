using AuthApi.Data;
using AuthApi.Models.Dtos;
using AuthApi.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuthApi.Services
{
    public class AuthService
        : IAuthService
    {
        private readonly AuthDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthService(AuthDbContext dbContext,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager) 
        {
            _db = dbContext;
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
        }

        public async Task<(string?, bool)> LoginAsync(UserLoginRequest userLogin)
        {
            try
            {
                var appUser = await _db.ApplicationUsers.SingleOrDefaultAsync(u => u.UserName == userLogin.Username);
                var siginIn = await _userManager.CheckPasswordAsync(appUser, userLogin.Password);

                if (siginIn)
                {
                    return (appUser.Id, true);
                }
                
                return ("Incorrect Username/Password!", false);
            }
            catch
            {
                throw;
            }
        }

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
