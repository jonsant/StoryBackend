using StoryBackend.Abstract;

namespace StoryBackend.Services
{
    public class StoryService : IStoryService
    {
        public IEnumerable<WeatherForecast> GetStoryBackendTest()
        {
            var summaries = new[]
{
                "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
            };

            IEnumerable<WeatherForecast> forecast = Enumerable.Range(1, 5).Select(index =>
                    new WeatherForecast
                    (
                        DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                        Random.Shared.Next(-20, 55),
                        summaries[Random.Shared.Next(summaries.Length)]
                    )).ToList();
            return forecast;
        }
    }
}
