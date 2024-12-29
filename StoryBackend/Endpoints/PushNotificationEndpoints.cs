using Microsoft.AspNetCore.Authorization;
using StoryBackend.CommandsAndQueries;

namespace StoryBackend.Endpoints;

public static class PushNotificationEndpoints
{
    public static WebApplication? UsePushNotificationEndpoints(this WebApplication app)
    {
        app.MapPost("/AddUserPushNotificationToken", PushNotificationCommandsAndQueries.HandleAddUserPushNotificationToken)
            .WithName("AddUserPushNotificationToken").WithOpenApi().RequireAuthorization();
        app.MapDelete("/DeleteUserPushNotificationToken/{token}", PushNotificationCommandsAndQueries.HandleDeleteUserPushNotificationToken)
            .WithName("DeleteUserPushNotificationToken").WithOpenApi().RequireAuthorization();
        return app;
    }
}
