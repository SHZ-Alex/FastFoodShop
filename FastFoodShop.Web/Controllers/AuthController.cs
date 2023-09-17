using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using FastFoodShop.Web.Models;
using FastFoodShop.Web.Models.AuthDtos;
using FastFoodShop.Web.Services.IServices;
using FastFoodShop.Web.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace FastFoodShop.Web.Controllers;

public class AuthController : Controller
{
    private readonly IAuthService _authService;
    private readonly ITokenProvider _tokenProvider;
    
    
    private List<SelectListItem> RoleList =>  new List<SelectListItem>
    {
        new SelectListItem{Text=SD.RoleAdmin,Value=SD.RoleAdmin},
        new SelectListItem{Text=SD.RoleClient,Value=SD.RoleClient}
    };

    public AuthController(IAuthService authService, ITokenProvider tokenProvider)
    {
        _authService = authService;
        _tokenProvider = tokenProvider;
    }

    public IActionResult Login()
    {
        LoginRequestDto loginRequestDto = new LoginRequestDto();
        return View(loginRequestDto);
    }
    
    public IActionResult Register()
    {
        ViewBag.RoleList = RoleList;
        
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> Register(RegistrationRequestDto request)
    {
        ResponseDto responseDto = await _authService.RegisterAsync(request);
        ResponseDto assingRole;

        if(responseDto != null && responseDto.IsSuccess)
        {
            if (string.IsNullOrEmpty(request.Role))
                request.Role = SD.RoleClient;
            
            assingRole = await _authService.AssignRoleAsync(request);
            if (assingRole!=null && assingRole.IsSuccess)
            {
                TempData["success"] = "Регистрация успешна";
                return RedirectToAction(nameof(Login));
            }
        }

        ViewBag.RoleList = RoleList;
        TempData["error"] = responseDto.Message;
        return View(request);
    }
    
    [HttpPost]
    public async Task<IActionResult> Login(LoginRequestDto request)
    {
        ResponseDto responseDto = await _authService.LoginAsync(request);

        if (responseDto == null || !responseDto.IsSuccess)
        {
            TempData["error"] = responseDto.Message;
            return View(request);
        }
        
        LoginResponseDto loginResponseDto = 
            JsonConvert.DeserializeObject<LoginResponseDto>(Convert.ToString(responseDto.Result));
        
        _tokenProvider.SetToken(loginResponseDto.Token);
        
        ClaimsPrincipal claimsPrincipal = await _tokenProvider.SignInUserAsync(loginResponseDto);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

        return RedirectToAction("Index", "Home");
    }
    
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        _tokenProvider.ClearToken();
        return RedirectToAction("Index", "Home");
    }
}