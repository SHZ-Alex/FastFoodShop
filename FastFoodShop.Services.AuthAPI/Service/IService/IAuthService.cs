using FastFoodShop.Services.AuthAPI.Models.Dto;

namespace FastFoodShop.Services.AuthAPI.Service.IService;

public interface IAuthService
{
    Task<string> Register(RegistrationRequestDto request);
    Task<LoginResponseDto> Login(LoginRequestDto request);
    Task<bool> AssignRole(string email, string roleName);
}