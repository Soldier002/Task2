using Domain.Integration.Models;

namespace Domain.Integration.ApiClients
{
    public interface IOpenWeatherMapApiClient
    {
        Task<OpenWeatherApiResponse> GetWeatherInAllCities(CancellationToken ct);
    }
}