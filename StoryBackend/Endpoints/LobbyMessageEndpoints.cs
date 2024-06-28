using StoryBackend.CommandsAndQueries;

namespace StoryBackend.Endpoints;

public static class LobbyMessageEndpoints
{
    public static WebApplication? UseLobbyMessageEndpoints(this WebApplication app)
    {
        app.MapGet("/GetLobbyMessagesByStoryId/{storyId}", LobbyMessageCommandsAndQueries.HandleGetLobbyMessagesByStoryId).WithName("GetLobbyMessagesByStoryId").WithOpenApi();
        app.MapPost("/CreateLobbyMessage", LobbyMessageCommandsAndQueries.HandleCreateLobbyMessage).WithName("CreateLobbyMessage").WithOpenApi();//.RequireAuthorization("play_story");


        return app;
    }
}
