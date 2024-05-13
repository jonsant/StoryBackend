using StoryBackend.Models.DTOs;

namespace StoryBackend.Abstract;

public interface IStoryService
{
    public Task<IEnumerable<GetWeatherForecastDto>> GetForecastBackendTest();
    public Task<GetWeatherForecastDto> CreateForecastTest(CreateWeatherForecast createWeatherForecastDto);
}
