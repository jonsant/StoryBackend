using Microsoft.EntityFrameworkCore;
using StoryBackend.Models;

namespace StoryBackend.Database
{
    public class StoryDbContext : DbContext
    {
        public DbSet<WeatherForecast> WeatherForecasts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Story> Stories { get; set; }
        public DbSet<Invitee> Invitees { get; set; }
        public DbSet<LobbyMessage> LobbyMessages { get; set; }
        public DbSet<Participant> Participants { get; set; }
        public DbSet<StoryEntry> StoryEntries { get; set; }
        public DbSet<EmailWhitelist> EmailWhitelist { get; set; }
        public DbSet<UserPushNotificationToken> UserPushNotificationTokens { get; set; }

        public StoryDbContext(DbContextOptions<StoryDbContext> dbContextOptions) : base(dbContextOptions) {}
    }
}
