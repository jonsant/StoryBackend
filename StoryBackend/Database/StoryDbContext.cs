using Microsoft.EntityFrameworkCore;

namespace StoryBackend.Database
{
    public class StoryDbContext : DbContext
    {
        public DbSet<WeatherForecast> WeatherForecasts { get; set; }

        public StoryDbContext(DbContextOptions<StoryDbContext> dbContextOptions) : base(dbContextOptions) {}
    }
}
