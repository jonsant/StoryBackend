using StoryBackend.Commands;

namespace StoryBackend.Endpoints;

public static class StoryEndpoints
{
    public static WebApplication? UseStoryEndpoints(this WebApplication app)
    {
        app.MapGet("/GetForecastBackendTest", StoryCommandsAndQueries.HandleGetForecastBackendTest).WithName("GetForecastBackendTest").WithOpenApi().RequireAuthorization("play_story");
        app.MapPost("/CreateForecastBackendTest", StoryCommandsAndQueries.HandleCreateForecastBackendTest).WithName("CreateForecastBackendTest").WithOpenApi().RequireAuthorization("play_story");

        return app;
    }
}
