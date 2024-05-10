using StoryBackend.Abstract;
using StoryBackend.Models.DTOs;

namespace StoryBackend.Commands
{
    public class StoryCommands
    {
        public static async Task<IEnumerable<GetWeatherForecastDto>> HandleGetForecastBackendTest(IStoryService storyService) => await storyService.GetForecastBackendTest();
        public static async Task<GetWeatherForecastDto> HandleCreateForecastBackendTest(IStoryService storyService, CreateWeatherForecast weatherForecastDto) => await storyService.CreateForecastTest(weatherForecastDto);
    }
}
