using StoryBackend.CommandsAndQueries;

namespace StoryBackend.Endpoints;

public static class InviteeEndpoints
{
    public static WebApplication? UseInviteeEndpoints(this WebApplication app)
    {
        app.MapGet("/GetStoryInvites", InviteeCommandsAndQueries.HandleGetStoryInvites).WithName("GetStoryInvites").WithOpenApi().RequireAuthorization();
        app.MapPost("/AcceptInvite", InviteeCommandsAndQueries.HandleAcceptInvite).WithName("AcceptInvite").WithOpenApi().RequireAuthorization();


        return app;
    }
}
