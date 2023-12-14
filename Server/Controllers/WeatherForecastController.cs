using Microsoft.AspNetCore.Mvc;
using InsecureProject.Database;

namespace InsecureProject.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController(DbModelContext dbContext, ILogger<WeatherForecastController> logger) : ControllerBase
{
    [HttpGet("{city}")]
    public IEnumerable<WeatherForecast> Get([FromRoute]string city)
    {
        logger.LogDebug($"Get called for city {city}.");

        var results = dbContext.Forecasts.Where(f => f.City.ToLower().Equals(city.ToLower())).ToList();
        var transformed = results 
            .Select(f => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                TemperatureC = f.Temperature,
                Summary = f.Summary
            });

        return transformed;
    }

    [HttpGet("insecure/{city}")]
    public IEnumerable<WeatherForecast> GetInsecure([FromRoute]string city)
    {
        logger.LogDebug($"Get insecure called for city {city}.");

        var results = dbContext.Forecasts.Where(f => f.City.ToLower().Equals(city.ToLower())).ToList();

        var transformed = results.Select(f => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                TemperatureC = f.Temperature,
                Summary = f.Summary
            }).ToList();
        return transformed;
    }
}
