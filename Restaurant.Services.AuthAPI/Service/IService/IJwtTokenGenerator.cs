using Restaurant.Services.AuthAPI.Models;

namespace Restaurant.Services.AuthAPI.Service.IService
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(ApplicationUser applicationUser);
    }
}
