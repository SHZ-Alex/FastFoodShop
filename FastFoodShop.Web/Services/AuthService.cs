using FastFoodShop.Web.Models;
using FastFoodShop.Web.Models.AuthDtos;
using FastFoodShop.Web.Models.Enums;
using FastFoodShop.Web.Services.IServices;
using FastFoodShop.Web.Utility;

namespace FastFoodShop.Web.Services;

public class AuthService : IAuthService
{
    private const string Url = "/api/auth/";
    private readonly IBaseService _baseService;

    public AuthService(IBaseService baseService)
    {
        _baseService = baseService;
    }
    public async Task<ResponseDto?> LoginAsync(LoginRequestDto loginDto)
    {
        return await _baseService.SendAsync(new RequestDto
        {
            ApiType = ApiType.POST,
            Data = loginDto,
            Url = SD.AuthAPIBase + Url + "login"
        }, withBearer: false);
    }

    public async Task<ResponseDto?> RegisterAsync(RegistrationRequestDto registerDto)
    {
        return await _baseService.SendAsync(new RequestDto
        {
            ApiType = ApiType.POST,
            Data = registerDto,
            Url = SD.AuthAPIBase + Url + "register"
        }, withBearer: false);
    }

    public async Task<ResponseDto?> AssignRoleAsync(RegistrationRequestDto roleDto)
    {
        return await _baseService.SendAsync(new RequestDto
        {
            ApiType = ApiType.POST,
            Data = roleDto,
            Url = SD.AuthAPIBase + Url + "assign-role"
        });
    }
}