using StoryBackend.CommandsAndQueries;

namespace StoryBackend.Endpoints;

public static class UserEndpoints
{
    public static WebApplication? UseUserEndpoints(this WebApplication app)
    {
        //app.MapGet("/GetUsers", UserCommandsAndQueries.HandleGetUsers).WithName("GetUsers").WithOpenApi().RequireAuthorization();
        app.MapGet("/GetUserById/{GlobalUserId}", UserCommandsAndQueries.HandleGetUserById).WithName("GetUserById").WithOpenApi().RequireAuthorization();
        app.MapGet("/GetUserByName/{Username}", UserCommandsAndQueries.HandleGetUserByName).WithName("GetUserByName").WithOpenApi().RequireAuthorization();
        app.MapPut("/ChangeUsername", UserCommandsAndQueries.HandleChangeUsername).WithName("ChangeUsername").WithOpenApi().RequireAuthorization();
        app.MapGet("/UsernameAvailable/{Username}", UserCommandsAndQueries.HandleUsernameAvailable).WithName("UsernameAvailable").WithOpenApi().RequireAuthorization();
        //app.MapPost("/CreateUser", UserCommandsAndQueries.HandleCreateUser).WithName("CreateUser").WithOpenApi().RequireAuthorization();

        return app;
    }
}
