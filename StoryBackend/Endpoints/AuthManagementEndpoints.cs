using StoryBackend.CommandsAndQueries;

namespace StoryBackend.Endpoints;

public static class AuthManagementEndpoints
{
    public static WebApplication? UseAuthManagementEndpoints(this WebApplication app)
    {
        app.MapPost("/Register", AuthManagementCommandsAndQueries.HandleRegister).WithName("Register").WithOpenApi();
        app.MapPost("/Login", AuthManagementCommandsAndQueries.HandleLogin).WithName("Login").WithOpenApi();
        app.MapPost("/CreateRole", AuthManagementCommandsAndQueries.HandleCreateRole).WithName("CreateRole").WithOpenApi().RequireAuthorization("AdminsOnly");
        app.MapGet("/GetRoles", AuthManagementCommandsAndQueries.HandleGetRoles).WithName("GetRoles").WithOpenApi().RequireAuthorization("AdminsOnly");

        return app;
    }
}
