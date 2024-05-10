public class WeatherForecast
{   
    public Guid WeatherForecastId { get; set; }
    public DateTimeOffset Date { get; set; }
    public int TemperatureC { get; set; }
    public string? Summary { get; set; } = null!;
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    public static WeatherForecast Instance(DateTimeOffset date, int TemperatureC, string? Summary) => new WeatherForecast
    {
        Date = date,
        TemperatureC = TemperatureC,
        Summary = Summary
    };
}
