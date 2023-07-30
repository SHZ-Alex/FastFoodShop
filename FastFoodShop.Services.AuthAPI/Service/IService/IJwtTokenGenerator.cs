using FastFoodShop.Services.AuthAPI.Models;

namespace FastFoodShop.Services.AuthAPI.Service.IService;

public interface IJwtTokenGenerator
{
    string GenerateToken(ApplicationUser applicationUser, IEnumerable<string> roles);
}