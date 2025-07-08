namespace AuthApi.Models.Dtos
{
    public class UserLoginRequest
    {
        public required string Username { get; set; } = string.Empty;
        public required string Password { get; set; } = string.Empty;
    }
}
