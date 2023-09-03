using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace FastFoodShop.Services.OrderAPI.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder AddAppAuthetication(this WebApplicationBuilder builder)
    {
        IConfigurationSection settingsSection = builder.Configuration.GetSection("ApiSettings");

        string secret = settingsSection.GetValue<string>("Secret");
        string issuer = settingsSection.GetValue<string>("Issuer");
        string audience = settingsSection.GetValue<string>("Audience");

        var key = Encoding.ASCII.GetBytes(secret);


        builder.Services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(x =>
        {
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = issuer,
                ValidAudience = audience,
                ValidateAudience = true
            };
        });

        return builder;
    }
}