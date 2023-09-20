using FastFoodShop.Services.RewardAPI.Models.Messages;

namespace FastFoodShop.Services.RewardAPI.Services.IServices;

public interface IRewardService
{
    Task UpdateRewards(RewardMessage rewardsMessage);
}