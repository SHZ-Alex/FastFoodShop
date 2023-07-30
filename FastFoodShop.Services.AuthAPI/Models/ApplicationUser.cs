using Microsoft.AspNetCore.Identity;

namespace FastFoodShop.Services.AuthAPI.Models;

public class ApplicationUser : IdentityUser
{
    public string Name { get; set; }
}