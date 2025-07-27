namespace IdentityApi.Models.Dtos;

public class BaseUserRequest
{
    public string Password { get; set; } = string.Empty;
}

public class UserNameLoginRequest
    : BaseUserRequest
{
    public string Username { get; set; } = string.Empty;
}

public class UserLoginRequest
    : BaseUserRequest
{
    public string Email { get; set; } = string.Empty;
}
