using Microsoft.AspNetCore.Identity;

namespace IdentityApi.Models.Entities
{
    public class ApplicationUser
        : IdentityUser
    {
        public string Name { get; set; } = string.Empty;
    }
}
