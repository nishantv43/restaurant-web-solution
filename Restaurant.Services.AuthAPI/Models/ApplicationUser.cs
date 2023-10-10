using Microsoft.AspNetCore.Identity;

namespace Restaurant.Services.AuthAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
    }
}
