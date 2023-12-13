using Microsoft.AspNetCore.Mvc;
using InsecureProject.Database;
using Microsoft.EntityFrameworkCore;

namespace InsecureProject.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController(DbModelContext dbContext, ILogger<WeatherForecastController> logger) : ControllerBase
{
    // private static readonly string[] Summaries = {
    //     "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    // };

    [HttpGet("{city}")]
    public IEnumerable<WeatherForecast> Get([FromRoute]string city)
    {
        logger.LogDebug($"Get called for city {city}.");
        // return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        // {
        //     Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
        //     TemperatureC = Random.Shared.Next(-20, 55),
        //     Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        // })
        // .ToArray();

        return dbContext.Forecasts.Where(f => f.City.ToLower().Equals(city.ToLower()))
            .Select(f => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                TemperatureC = f.Temperature,
                Summary = f.Summary
            });
    }

    [HttpGet("insecure/{city}")]
    public IEnumerable<WeatherForecast> GetInsecure([FromRoute]string city)
    {
        logger.LogDebug($"Get insecure called for city {city}.");

        var results = dbContext.Forecasts.FromSqlRaw(
            $"select * from Forecasts where lower(City) = lower('{city}');")
            .ToList();
        var transformed = results.Select(f => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                TemperatureC = f.Temperature,
                Summary = f.Summary
            }).ToList();
        return transformed;
    }
}
