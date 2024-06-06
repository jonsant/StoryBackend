using StoryBackend.CommandsAndQueries;

namespace StoryBackend.Endpoints;

public static class StoryEndpoints
{
    public static WebApplication? UseStoryEndpoints(this WebApplication app)
    {
        app.MapGet("/GetForecastBackendTest", StoryCommandsAndQueries.HandleGetForecastBackendTest).WithName("GetForecastBackendTest").WithOpenApi().RequireAuthorization("play_story");
        app.MapGet("/GetStoryById/{storyId}", StoryCommandsAndQueries.HandleGetStoryById).WithName("GetStoryById").WithOpenApi().RequireAuthorization("play_story");
        app.MapGet("/GetStoriesByUserId", StoryCommandsAndQueries.HandleGetStoriesByUserId).WithName("GetStoriesByUserId").WithOpenApi().RequireAuthorization("play_story");
        app.MapPost("/CreateForecastBackendTest", StoryCommandsAndQueries.HandleCreateForecastBackendTest).WithName("CreateForecastBackendTest").WithOpenApi().RequireAuthorization("play_story");
        app.MapPost("/CreateStory", StoryCommandsAndQueries.HandleCreateStory).WithName("CreateStory").WithOpenApi().RequireAuthorization("play_story");

        return app;
    }
}
