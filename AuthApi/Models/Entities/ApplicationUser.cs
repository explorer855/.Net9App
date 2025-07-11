using Microsoft.AspNetCore.Identity;

namespace AuthApi.Models.Entities
{
    public class ApplicationUser
        : IdentityUser
    {
        public string Name { get; set; } = string.Empty;
    }
}
