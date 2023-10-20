using Microsoft.EntityFrameworkCore;
using Restaurant.Services.RewardsAPI.Data;
using Restaurant.Services.RewardsAPI.Message;
using Restaurant.Services.RewardsAPI.Models;
using Restaurant.Services.RewardsAPI.Service;
using System.Text;

namespace Restaurant.Services.RewardsAPI.Service
{
    public class RewardsService : IRewardsService
    {
        private DbContextOptions<AppDbContext> _dbOptions;

        public RewardsService(DbContextOptions<AppDbContext> dbOptions)
        {
            _dbOptions = dbOptions;
        }
        public async Task UpdateRewards(RewardsMessage rewardsMessage)
        {
            try
            {
                Rewards rewards = new()
                {
                    OrderId = rewardsMessage.OrderId,
                    RewardsActivity = rewardsMessage.RewardsActivity,
                    UserId = rewardsMessage.UserId,
                    RewardsDate = DateTime.Now
                };
                await using var _db = new AppDbContext(_dbOptions);
                await _db.Rewards.AddAsync(rewards);
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
            }
        }
    }
}
