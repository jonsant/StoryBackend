using StoryBackend.Abstract;

namespace StoryBackend.Commands
{
    public class StoryCommands
    {
        public static IEnumerable<WeatherForecast> HandleGetStoryBackendTest(IStoryService storyService) => storyService.GetStoryBackendTest();
    }
}
