using ArchUnitPresentation.Services.Contracts.Models;
using ArchUnitPresentation.Services.Contracts.Services;
using ArchUnitPresentation.Services.Impl.Repositories;

namespace ArchUnitPresentation.Services.Impl.Services;

internal sealed class WeatherService: IWeatherService
{
    private readonly WeatherRepository weatherRepository;

    public WeatherService(WeatherRepository weatherRepository)
    {
        this.weatherRepository = weatherRepository;
    }

    public IEnumerable<WeatherForecastDto> GetWeather()
    {
        return weatherRepository.GetWeather();
    }
}