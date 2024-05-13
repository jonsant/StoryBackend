using StoryBackend.Commands;

namespace StoryBackend.Endpoints;

public static class UserEndpoints
{
    public static WebApplication? UseUserEndpoints(this WebApplication app)
    {
        app.MapGet("/GetUsers", UserCommands.HandleGetUsers).WithName("GetUsers").WithOpenApi().RequireAuthorization("play_story");
        app.MapGet("/GetUserById/{GlobalUserId}", UserCommands.HandleGetUserById).WithName("GetUserById").WithOpenApi().RequireAuthorization("play_story");
        app.MapPost("/CreateUser", UserCommands.HandleCreateUser).WithName("CreateUser").WithOpenApi().RequireAuthorization("play_story");

        return app;
    }
}
