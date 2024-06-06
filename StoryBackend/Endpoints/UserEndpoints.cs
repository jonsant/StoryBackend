using StoryBackend.CommandsAndQueries;

namespace StoryBackend.Endpoints;

public static class UserEndpoints
{
    public static WebApplication? UseUserEndpoints(this WebApplication app)
    {
        app.MapGet("/GetUsers", UserCommandsAndQueries.HandleGetUsers).WithName("GetUsers").WithOpenApi().RequireAuthorization("play_story");
        app.MapGet("/GetUserById/{GlobalUserId}", UserCommandsAndQueries.HandleGetUserById).WithName("GetUserById").WithOpenApi().RequireAuthorization("play_story");
        app.MapGet("/GetUserByName/{Username}", UserCommandsAndQueries.HandleGetUserByName).WithName("GetUserByName").WithOpenApi().RequireAuthorization("play_story");
        app.MapPost("/CreateUser", UserCommandsAndQueries.HandleCreateUser).WithName("CreateUser").WithOpenApi().RequireAuthorization("play_story");

        return app;
    }
}
