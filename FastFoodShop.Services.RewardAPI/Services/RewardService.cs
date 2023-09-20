using FastFoodShop.Services.RewardAPI.Data;
using FastFoodShop.Services.RewardAPI.Models;
using FastFoodShop.Services.RewardAPI.Models.Messages;
using FastFoodShop.Services.RewardAPI.Services.IServices;
using Microsoft.EntityFrameworkCore;

namespace FastFoodShop.Services.RewardAPI.Services;

public class RewardService : IRewardService
{
    private readonly DbContextOptions<AppDbContext> _dbOptions;

    public RewardService(DbContextOptions<AppDbContext> dbOptions)
    {
        _dbOptions = dbOptions;
    }

    public async Task UpdateRewards(RewardMessage rewardsMessage)
    {
        try
        {
            Reward reward = new()
            {
                OrderId = rewardsMessage.OrderId,
                RewardsActivity = rewardsMessage.RewardsActivity,
                UserId = rewardsMessage.UserId,
                RewardsDate = DateTime.Now
            };
            
            await using var _db = new AppDbContext(_dbOptions);
            _db.Rewards.Add(reward);
            await _db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
        }
    }
}