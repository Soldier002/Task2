using Newtonsoft.Json;
using Quartz;
using static Task2.Server.Controllers.WeatherForecastController;

namespace Task2.Server.Jobs
{
    public class GetWeatherDataJob : IJob
    {
        private readonly HttpClient _httpClient;

        public GetWeatherDataJob(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("WeatherApi");
        }

        public async Task Execute(IJobExecutionContext context)
        {
            // Code that sends a periodic email to the user (for example)
            // Note: This method must always return a value 
            // This is especially important for trigger listeners watching job execution 
            var result = await _httpClient.GetAsync("https://api.openweathermap.org/data/2.5/group?id=2643743,3333169,6167865,6094817,361058,360630&appid=faa625de9ce05a0abdf9cf5850ca5637&units=metric");

            var result2 = JsonConvert.DeserializeObject<Root>(await result.Content.ReadAsStringAsync());
        }
    }
}
