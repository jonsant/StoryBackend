using Microsoft.EntityFrameworkCore;
using StoryBackend.Models;

namespace StoryBackend.Database
{
    public class StoryDbContext : DbContext
    {
        public DbSet<WeatherForecast> WeatherForecasts { get; set; }
        public DbSet<User> Users { get; set; }

        public StoryDbContext(DbContextOptions<StoryDbContext> dbContextOptions) : base(dbContextOptions) {}
    }
}
