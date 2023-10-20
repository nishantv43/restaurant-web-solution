using Microsoft.EntityFrameworkCore;
using Restaurant.Services.RewardsAPI.Models;

namespace Restaurant.Services.RewardsAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Rewards> Rewards { get; set; }

    }
}