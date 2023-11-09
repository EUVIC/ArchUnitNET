using ArchUnitPresentation.Services.Contracts.Models;
using ArchUnitPresentation.Services.Contracts.Services;
using Microsoft.AspNetCore.Mvc;

namespace ArchUnitPresentation.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> logger;
    private readonly IWeatherService weatherService;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherService weatherService)
    {
        this.logger = logger;
        this.weatherService = weatherService;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecastDto> Get()
    {
        return weatherService.GetWeather();
    }
}