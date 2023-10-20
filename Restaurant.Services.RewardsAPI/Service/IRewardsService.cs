using Restaurant.Services.RewardsAPI.Message;

namespace Restaurant.Services.RewardsAPI.Service
{
    public interface IRewardsService
    {
        Task UpdateRewards(RewardsMessage rewardsMessage);
    }
}
