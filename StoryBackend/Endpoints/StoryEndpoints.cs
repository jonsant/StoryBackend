using StoryBackend.Commands;

namespace StoryBackend.Endpoints;

public static class StoryEndpoints
{
    public static WebApplication? UseStoryEndpoints(this WebApplication app)
    {
        app.MapGet("/GetForecastBackendTest", StoryCommands.HandleGetForecastBackendTest).WithName("GetForecastBackendTest").WithOpenApi().RequireAuthorization("play_story");
        app.MapPost("/CreateForecastBackendTest", StoryCommands.HandleCreateForecastBackendTest).WithName("CreateForecastBackendTest").WithOpenApi().RequireAuthorization("play_story");

        return app;
    }
}
