using StoryBackend.CommandsAndQueries;

namespace StoryBackend.Endpoints;

public static class StoryEndpoints
{
    public static WebApplication? UseStoryEndpoints(this WebApplication app)
    {
        app.MapGet("/GetForecastBackendTest", StoryCommandsAndQueries.HandleGetForecastBackendTest).WithName("GetForecastBackendTest").WithOpenApi().RequireAuthorization();
        app.MapGet("/GetStoryById/{storyId}", StoryCommandsAndQueries.HandleGetStoryById).WithName("GetStoryById").WithOpenApi().RequireAuthorization();
        app.MapGet("/GetParticipantStoriesByUserId", StoryCommandsAndQueries.HandleGetParticipantStoriesByUserId).WithName("GetParticipantStoriesByUserId").WithOpenApi().RequireAuthorization();
        app.MapGet("/GetStoriesByUserId", StoryCommandsAndQueries.HandleGetStoriesByUserId).WithName("GetStoriesByUserId").WithOpenApi().RequireAuthorization();
        app.MapPost("/CreateForecastBackendTest", StoryCommandsAndQueries.HandleCreateForecastBackendTest).WithName("CreateForecastBackendTest").WithOpenApi().RequireAuthorization();
        app.MapPost("/CreateStory", StoryCommandsAndQueries.HandleCreateStory).WithName("CreateStory").WithOpenApi().RequireAuthorization();
        app.MapPut("/StartStory", StoryCommandsAndQueries.HandleStartStory).WithName("StartStory").WithOpenApi().RequireAuthorization();
        app.MapPost("/EndStory", StoryCommandsAndQueries.HandleEndStory).WithName("EndStory").WithOpenApi().RequireAuthorization();
        app.MapPost("/CreateEntry", StoryCommandsAndQueries.HandleCreateEntry).WithName("CreateEntry").WithOpenApi().RequireAuthorization();

        return app;
    }
}
