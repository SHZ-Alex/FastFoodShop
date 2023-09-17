using System.ComponentModel.DataAnnotations;

namespace FastFoodShop.Web.Models.AuthDtos;

public class LoginRequestDto
{
    [Required]
    public string UserName { get; set; }
    [Required]
    public string Password { get; set; }
}