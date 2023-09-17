using System.Security.Claims;
using FastFoodShop.Web.Models;
using FastFoodShop.Web.Models.AuthDtos;

namespace FastFoodShop.Web.Services.IServices;

public interface ITokenProvider
{
    void SetToken(string token);
    string? GetToken();
    void ClearToken();
    Task<ClaimsPrincipal> SignInUserAsync(LoginResponseDto model);
}