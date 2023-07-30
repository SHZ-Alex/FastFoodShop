using FastFoodShop.Web.Models;

namespace FastFoodShop.Web.Services.IServices;

public interface IAuthService
{
    Task<ResponseDto?> LoginAsync(LoginRequestDto loginDto);
    Task<ResponseDto?> RegisterAsync(RegistrationRequestDto registerDto);
    Task<ResponseDto?> AssignRoleAsync(RegistrationRequestDto roleDto);
}