using Microsoft.AspNetCore.Identity;

namespace ServicePortal.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string DisplayName { get; set; } = string.Empty;
    }
}