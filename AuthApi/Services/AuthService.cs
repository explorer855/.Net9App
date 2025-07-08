using AuthApi.Data;
using AuthApi.Data.Models;
using Microsoft.AspNetCore.Identity;

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
            _roleManager = roleManager;
        }

        public Task<string> LoginAsync(UserLoginModel userLogin)
        {
            throw new NotImplementedException();
        }

        public async Task<string> RegisterAsync(RegisterUserModel registerUser)
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
