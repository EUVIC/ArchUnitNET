using System.Runtime.CompilerServices;
using ArchUnitPresentation.Services.Contracts.Services;
using ArchUnitPresentation.Services.Impl.Repositories;
using ArchUnitPresentation.Services.Impl.Services;
using Microsoft.Extensions.DependencyInjection;

[assembly:InternalsVisibleTo("ArchUnitPresentation.Tests")]

namespace ArchUnitPresentation.Services.Impl;

public static class ServiceRegistrations
{
    public static IServiceCollection RegisterServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<IWeatherService, WeatherService>();
        serviceCollection.AddTransient<WeatherRepository>();
        return serviceCollection;
    }
}