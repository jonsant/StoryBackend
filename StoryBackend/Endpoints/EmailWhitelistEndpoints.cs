using Microsoft.AspNetCore.Authorization;
using StoryBackend.CommandsAndQueries;

namespace StoryBackend.Endpoints;

public static class EmailWhitelistEndpoints
{
    public static WebApplication? UseEmailWhitelistEndpoints(this WebApplication app)
    {
        app.MapPost("/AddEmail", EmailWhitelistCommandsAndQueries.HandleAddEmail)
            .WithName("AddEmail").WithOpenApi().RequireAuthorization("AdminsOnly");
        return app;
    }
}
