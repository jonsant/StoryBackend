using StoryBackend.Commands;

namespace StoryBackend.Endpoints
{
    public static class StoryEndpoints
    {
        public static WebApplication? UseStoryEndpoints(this WebApplication app)
        {
            app.MapGet("/GetForecastBackendTest", StoryCommands.HandleGetForecastBackendTest).WithName("GetForecastBackendTest").WithOpenApi();
            app.MapPost("/CreateForecastBackendTest", StoryCommands.HandleCreateForecastBackendTest).WithName("CreateForecastBackendTest").WithOpenApi();

            return app;
        }
    }
}
