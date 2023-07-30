using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FastFoodShop.Services.AuthAPI.Models;
using FastFoodShop.Services.AuthAPI.Service.IService;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace FastFoodShop.Services.AuthAPI.Service;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly JwtOptions _jwpOption;

    public JwtTokenGenerator(IOptions<JwtOptions> jwtOptions)
    {
        _jwpOption = jwtOptions.Value;
    }
    
    public string GenerateToken(ApplicationUser applicationUser, IEnumerable<string> roles)
    {
        JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

        byte[] key = Encoding.ASCII.GetBytes(_jwpOption.Secret);

        List<Claim> claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Email, applicationUser.Email),
            new Claim(JwtRegisteredClaimNames.Sub, applicationUser.Id),
            new Claim(JwtRegisteredClaimNames.Name, applicationUser.UserName),
        };
        
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        SecurityTokenDescriptor tokenDescription = new SecurityTokenDescriptor
        {
            Audience = _jwpOption.Audience,
            Issuer = _jwpOption.Issuer,
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(7),
            SigningCredentials = new SigningCredentials
                (new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        SecurityToken token = handler.CreateToken(tokenDescription);
        return handler.WriteToken(token);
    }
}