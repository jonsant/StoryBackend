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
    }
}
