using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using FastFoodShop.Web.Models;
using FastFoodShop.Web.Models.AuthDtos;
using FastFoodShop.Web.Services.IServices;
using FastFoodShop.Web.Utility;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace FastFoodShop.Web.Services;

public class TokenProvider : ITokenProvider
{
    private readonly IHttpContextAccessor _contextAccessor;

    public TokenProvider(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }
    
    public void SetToken(string token)
    {
        _contextAccessor.HttpContext?.Response.Cookies.Append(SD.TokenCookie, token);
    }

    public string? GetToken()
    {
        string? token = null;
        bool? hasToken = _contextAccessor.HttpContext?.Request.Cookies.TryGetValue(SD.TokenCookie, out token);
        return hasToken == true ? token : null;
    }

    public void ClearToken()
    {
        _contextAccessor.HttpContext?.Response.Cookies.Delete(SD.TokenCookie);
    }
    
    public async Task<ClaimsPrincipal> SignInUserAsync(LoginResponseDto request)
    {
        JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

        JwtSecurityToken jwt = handler.ReadJwtToken(request.Token); 

        ClaimsIdentity identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
        
        identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email, 
            jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));
        identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub,
            jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub).Value));
        identity.AddClaim(new Claim(JwtRegisteredClaimNames.Name,
            jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Name).Value));
        
        identity.AddClaim(new Claim(ClaimTypes.Name,
            jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));
        identity.AddClaim(new Claim(ClaimTypes.Role,
            jwt.Claims.FirstOrDefault(u => u.Type == "role").Value));
        
        return new ClaimsPrincipal(identity);
    }
}