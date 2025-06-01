using Microsoft.EntityFrameworkCore;
using StoryBackend.Models;

namespace StoryBackend.Database
{
    public static class Extensions
    {
        public static IServiceCollection AddStoryDb(this IServiceCollection serviceCollection, WebApplicationBuilder builder)
        {
            serviceCollection.AddDbContext<StoryDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("StoryDb"));
            }, ServiceLifetime.Transient);
            serviceCollection.AddDbContext<IdStoryDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("IdStoryDb"));
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
            string? autoAdmins = app.Configuration.GetValue<string?>("AutoAdmins:Emails");
            if (autoAdmins is null) return app;
            string firstAdmin = autoAdmins.Split(";")[0];
            EmailWhitelist? adminWhitelisted = storyDbContext.EmailWhitelist.FirstOrDefault(w => w.Email.Equals(firstAdmin));
            if (adminWhitelisted is null)
            {
                storyDbContext.EmailWhitelist.Add(EmailWhitelist.Instance(firstAdmin));
                storyDbContext.SaveChanges();
            }
            return app;
        }

        public static WebApplication ApplyIdStoryMigrations(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var idStoryDbContext = scope.ServiceProvider.GetRequiredService<IdStoryDbContext>();
            IEnumerable<string>? pending = idStoryDbContext.Database.GetPendingMigrations();
            if (pending.Any())
            {
                idStoryDbContext.Database.Migrate();
            }
            return app;
        }
    }
}
