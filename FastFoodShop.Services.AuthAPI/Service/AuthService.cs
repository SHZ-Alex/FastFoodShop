using FastFoodShop.Services.AuthAPI.Data;
using FastFoodShop.Services.AuthAPI.Models;
using FastFoodShop.Services.AuthAPI.Models.Dto;
using FastFoodShop.Services.AuthAPI.Service.IService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FastFoodShop.Services.AuthAPI.Service;

public class AuthService : IAuthService
{
    private readonly AppDbContext _db;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public AuthService(AppDbContext db, 
        RoleManager<IdentityRole> roleManager, 
        UserManager<ApplicationUser> userManager,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _db = db;
        _roleManager = roleManager;
        _userManager = userManager;
        _jwtTokenGenerator = jwtTokenGenerator;
    }
    
    public async Task<string> Register(RegistrationRequestDto request)
    {
        ApplicationUser newUser = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            NormalizedEmail = request.Email.ToUpper(),
            Name = request.Name,
            PhoneNumber = request.PhoneNumber
        };

        try
        {
            IdentityResult createResult = await _userManager.CreateAsync(newUser, request.Password);

            if (createResult.Succeeded)
            {
                ApplicationUser userToReturn = await _db.ApplicationUsers
                    .FirstAsync(x => x.UserName == request.Email);

                UserDto userDto = new UserDto
                {
                    Email = userToReturn.Email,
                    Id = userToReturn.Id,
                    Name = userToReturn.Name,
                    PhoneNumber = userToReturn.PhoneNumber
                };

                return "";
            }
            else
            {
                return createResult.Errors.FirstOrDefault().Description;
            }
        }
        catch (Exception)
        {

        }
        return "Error encountered";
    }

    public async Task<LoginResponseDto> Login(LoginRequestDto request)
    {
        ApplicationUser user = await _db.ApplicationUsers
            .SingleOrDefaultAsync(x => x.UserName.ToLower() == request.UserName.ToLower());

        if (user == null)
            return new LoginResponseDto { User = null, Token = "" };
        
        if (!await _userManager.CheckPasswordAsync(user, request.Password))
            return new LoginResponseDto { User = null, Token = "" };

        IEnumerable<string> roles = await _userManager.GetRolesAsync(user);
        string token = _jwtTokenGenerator.GenerateToken(user, roles);

        UserDto userDto = new UserDto
        {
            Email = user.Email,
            Id = user.Id,
            Name = user.Name,
            PhoneNumber = user.PhoneNumber
        };

        return new LoginResponseDto
        {
            User = userDto,
            Token = token
        };
    }

    public async Task<bool> AssignRole(string email, string roleName)
    {
        ApplicationUser user = await _db.ApplicationUsers
            .SingleOrDefaultAsync(x => x.Email.ToLower() == email.ToLower());

        if (user == null)
            return false;

        if (! await _roleManager.RoleExistsAsync(roleName))
            await _roleManager.CreateAsync(new IdentityRole(roleName));

        await _userManager.AddToRoleAsync(user, roleName);
        return true;
    }
}