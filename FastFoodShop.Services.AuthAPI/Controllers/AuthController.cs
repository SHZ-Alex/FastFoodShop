using FastFoodShop.Services.AuthAPI.Models.Dto;
using FastFoodShop.Services.AuthAPI.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace FastFoodShop.Services.AuthAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    protected ResponseDto _response;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
        _response = new ResponseDto();
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegistrationRequestDto request)
    {
        string errorMessage = await _authService.Register(request);
        
        if (!errorMessage.IsNullOrEmpty())
        {
            _response.IsSuccess = false;
            _response.Message = errorMessage;
            return BadRequest(_response);
        }
        
        return Ok(_response);
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequestDto request)
    {
        LoginResponseDto loginResponse = await _authService.Login(request);

        if (loginResponse.User == null)
        {
            _response.IsSuccess = false;
            _response.Message = "Username or password is incorrect";
            return BadRequest(_response);
        }

        _response.Result = loginResponse;
        return Ok(_response);
    }
    
    [HttpPost("assign-role")]
    public async Task<IActionResult> AssignRole([FromBody] RegistrationRequestDto request)
    {
        var assignRoleSuccessful = await _authService.AssignRole(request.Email, request.Role.ToLower());
        
        if (!assignRoleSuccessful)
        {
            _response.IsSuccess = false;
            _response.Message = "Error encountered";
            return BadRequest(_response);
        }
        
        return Ok(_response);

    }

}