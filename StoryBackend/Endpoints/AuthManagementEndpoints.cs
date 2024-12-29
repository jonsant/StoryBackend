using StoryBackend.CommandsAndQueries;

namespace StoryBackend.Endpoints;

public static class AuthManagementEndpoints
{
    public static WebApplication? UseAuthManagementEndpoints(this WebApplication app)
    {
        app.MapPost("/Register", AuthManagementCommandsAndQueries.HandleRegister).WithName("Register").WithOpenApi();
        app.MapPost("/Login", AuthManagementCommandsAndQueries.HandleLogin).WithName("Login").WithOpenApi();
        app.MapPost("/ResetPasswordEmail", AuthManagementCommandsAndQueries.HandleResetPasswordEmail).WithName("ResetPasswordEmail").WithOpenApi();
        app.MapPost("/ResetPassword", AuthManagementCommandsAndQueries.HandleResetPassword).WithName("ResetPassword").WithOpenApi();
        app.MapPost("/CreateRole", AuthManagementCommandsAndQueries.HandleCreateRole).WithName("CreateRole").WithOpenApi().RequireAuthorization("AdminsOnly");
        app.MapGet("/GetRoles", AuthManagementCommandsAndQueries.HandleGetRoles).WithName("GetRoles").WithOpenApi().RequireAuthorization("AdminsOnly");
        app.MapGet("/GetCurrentUser", AuthManagementCommandsAndQueries.HandleGetCurrentUser).WithName("GetCurrentUser").WithOpenApi().RequireAuthorization();
        return app;
    }
}
