using Mapster;
using Microsoft.EntityFrameworkCore;
using StoryBackend.Abstract;
using StoryBackend.Database;
using StoryBackend.Models.DTOs;

namespace StoryBackend.Services
{
    public class StoryService(StoryDbContext storyDbContext) : IStoryService
    {
        public async Task<GetWeatherForecastDto> CreateForecastTest(CreateWeatherForecast createWeatherForecastDto)
        {
            WeatherForecast forecast = createWeatherForecastDto.Adapt<WeatherForecast>();

            storyDbContext.WeatherForecasts.Add(forecast);
            await storyDbContext.SaveChangesAsync();
            return await Task.FromResult(forecast.Adapt<GetWeatherForecastDto>());
        }

        public async Task<IEnumerable<GetWeatherForecastDto>> GetForecastBackendTest()
        {
            IEnumerable<GetWeatherForecastDto> forecasts = await storyDbContext.WeatherForecasts.Select(f => f.Adapt<GetWeatherForecastDto>()).ToListAsync();
            return await Task.FromResult(forecasts);
        }
    }
}
