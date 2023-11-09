using ArchUnitPresentation.Services.Contracts.Models;

namespace ArchUnitPresentation.Services.Contracts.Services;

public interface IWeatherService
{
    public IEnumerable<WeatherForecastDto> GetWeather();
}