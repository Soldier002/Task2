namespace Domain.Common.Configuration
{
    public interface IWeatherOverviewConfiguration
    {
        string DefaultConnectionString { get; }

        string CityIds { get; }

        string WeatherApiKey { get; }
    }
}