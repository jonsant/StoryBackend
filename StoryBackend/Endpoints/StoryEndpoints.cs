using StoryBackend.Abstract;
using StoryBackend.Commands;

namespace StoryBackend.Endpoints
{
    public static class StoryEndpoints
    {
        public static WebApplication? UseStoryEndpoints(this WebApplication app)
        {
            app.MapGet("/getstorybackendtest", StoryCommands.HandleGetStoryBackendTest).WithName("GetStoryBackendTest").WithOpenApi();

            return app;
        }
    }
}
