using FastFoodShop.Services.EmailAPI.Models.Dto;

namespace FastFoodShop.Services.EmailAPI.Services.IServices;

public interface IEmailService
{
    Task EmailCartAndLog(CartDto cartDto);
    Task RegisterUserEmailAndLog(string email);
    Task LogOrderPlaced(RewardMessage rewardsDto);
}