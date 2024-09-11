using Domain.Integration.Models;
using Domain.Persistence.Entities;
using Domain.Services.Mappers;

namespace Services.Mappers
{
    public class OpenWeatherMapApiResponseMapper : IOpenWeatherMapApiResponseMapper
    {
        public City ToCity(OpenWeather openWeather)
        {
            var city = new City
            {
                Id = openWeather.id,
                Name = openWeather.name,
                Country = openWeather.sys.country
            };

            return city;
        }

        public IList<WeatherReport> ToWeatherReports(OpenWeatherApiResponse apiResponse)
        {
            var weatherReports = new List<WeatherReport>();
            foreach (var w in apiResponse.list)
            {
                var weatherReport = new WeatherReport
                {
                    CityId = w.id,
                    MinTemp = w.main.temp_min,
                    MaxTemp = w.main.temp_max,
                };

                weatherReports.Add(weatherReport);
            }

            return weatherReports;
        }
    }
}
