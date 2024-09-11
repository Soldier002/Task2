using Common.Configuration;
using Domain.Common.Configuration;
using Domain.Integration.ApiClients;
using Domain.Integration.Models;
using Newtonsoft.Json;

namespace Integration.ApiClients
{
    public class OpenWeatherMapApiClient : IOpenWeatherMapApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _weatherApiUrl = "https://api.openweathermap.org/data/2.5/weather?q=London&appid=";
        private readonly IWeatherOverviewConfiguration _configuration;

        public OpenWeatherMapApiClient(IHttpClientFactory httpClientFactory, IWeatherOverviewConfiguration configuration)
        {
            _httpClient = httpClientFactory.CreateClient(CommonStrings.WeatherApi);
            _configuration = configuration;
        }

        public async Task<OpenWeatherApiResponse> GetWeatherInAllCities(CancellationToken ct)
        {
            var url = $"https://api.openweathermap.org/data/2.5/group?id={_configuration.CityIds}&appid={_configuration.WeatherApiKey}&units=metric";
            var response = await _httpClient.GetAsync(url, ct);
            var responseContent = await response.Content.ReadAsStringAsync(ct);
            var deserializedContent = JsonConvert.DeserializeObject<OpenWeatherApiResponse>(responseContent);
            return deserializedContent;
        }
    }
}
