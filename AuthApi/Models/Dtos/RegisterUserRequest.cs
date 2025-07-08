namespace AuthApi.Models.Dtos
{
    public class RegisterUserRequest
    {
        public required string Email { get; set; } = string.Empty;
        public required string Name { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
    }
}
