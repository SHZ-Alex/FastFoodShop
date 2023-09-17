using FastFoodShop.Web.Models;
using FastFoodShop.Web.Models.AuthDtos;

namespace FastFoodShop.Web.Services.IServices;

public interface IAuthService
{
    Task<ResponseDto?> LoginAsync(LoginRequestDto loginDto);
    Task<ResponseDto?> RegisterAsync(RegistrationRequestDto registerDto);
    Task<ResponseDto?> AssignRoleAsync(RegistrationRequestDto roleDto);
}