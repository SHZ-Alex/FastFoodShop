namespace FastFoodShop.Services.EmailAPI.Models.Dto;

public class RewardMessage
{
    public string UserId { get; set; }
    public int RewardsActivity { get; set; }
    public int OrderId { get; set; }
}