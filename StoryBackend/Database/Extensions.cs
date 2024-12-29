using Microsoft.EntityFrameworkCore;

namespace StoryBackend.Database
{
    public static class Extensions
    {
        public static IServiceCollection AddStoryDb(this IServiceCollection serviceCollection, WebApplicationBuilder builder)
        {
            serviceCollection.AddDbContext<StoryDbContext>(options =>
            {
                options.UseSqlite(builder.Configuration.GetConnectionString("StoryDb"));
            }, ServiceLifetime.Transient);
            serviceCollection.AddDbContext<IdStoryDbContext>(options =>
            {
                options.UseSqlite(builder.Configuration.GetConnectionString("IdStoryDb"));
            }, ServiceLifetime.Transient);

            return serviceCollection;
        }

        public static WebApplication ApplyStoryMigrations(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var storyDbContext = scope.ServiceProvider.GetRequiredService<StoryDbContext>();
            IEnumerable<string>? pending = storyDbContext.Database.GetPendingMigrations();
            if (pending.Any())
            {
                storyDbContext.Database.Migrate();
            }
            return app;
        }
    }
}
