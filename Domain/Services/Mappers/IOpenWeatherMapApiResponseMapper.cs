using Domain.Integration.Models;
using Domain.Persistence.Entities;

namespace Domain.Services.Mappers
{
    public interface IOpenWeatherMapApiResponseMapper
    {
        City ToCity(OpenWeather openWeather);
        IList<WeatherReport> ToWeatherReports(OpenWeatherApiResponse apiResponse);
    }
}