using AuthApi.Data.Models;

namespace AuthApi.Services
{
    public class AuthService
        : IAuthService
    {
        //private readonly identityuser

        public AuthService() { }

        public Task<string> LoginAsync(UserLoginModel userLogin)
        {
            throw new NotImplementedException();
        }

        public Task<string> RegisterAsync(RegisterUserModel registerUser)
        {
            throw new NotImplementedException();
        }
    }
}
