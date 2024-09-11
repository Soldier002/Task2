using Domain.Common.Configuration;
using Microsoft.Extensions.Configuration;
using System.Runtime.CompilerServices;


namespace Common.Configuration
{
    public class WeatherOverviewConfiguration : IWeatherOverviewConfiguration
    {
        private readonly IConfiguration _configuration;

        public WeatherOverviewConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string WeatherApiKey => GetConfig();

        public string CityIds => GetConfig();

        public string DefaultConnectionString => GetConnectionString();

        private string GetConnectionString([CallerMemberName] string callerMemberName = "")
        {
            if (string.IsNullOrEmpty(callerMemberName))
            {
                throw new ArgumentException($"{nameof(callerMemberName)} argument null or empty");
            }

            var value = _configuration.GetConnectionString(callerMemberName);
            if (string.IsNullOrEmpty(value))
            {
                throw new InvalidOperationException($"Configuration for key \"{callerMemberName}\" missing");
            }

            return value;
        }

        private string GetConfig([CallerMemberName] string callerMemberName = "")
        {
            if (string.IsNullOrEmpty(callerMemberName))
            {
                throw new ArgumentException($"{nameof(callerMemberName)} argument null or empty");
            }

            var value = _configuration[callerMemberName];
            if (string.IsNullOrEmpty(value))
            {
                throw new InvalidOperationException($"Configuration for key \"{callerMemberName}\" missing");
            }

            return value;
        }
    }
}
