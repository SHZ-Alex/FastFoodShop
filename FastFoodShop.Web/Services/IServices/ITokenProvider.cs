using System.Security.Claims;
using FastFoodShop.Web.Models;

namespace FastFoodShop.Web.Services.IServices;

public interface ITokenProvider
{
    void SetToken(string token);
    string? GetToken();
    void ClearToken();
    Task<ClaimsPrincipal> SignInUserAsync(LoginResponseDto model);
}