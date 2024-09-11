using Domain.Persistence.Entities;
using Domain.Persistence.Repositories;
using Newtonsoft.Json;
using Quartz;
using static Task2.Server.Controllers.WeatherOverviewController;

namespace Task2.Server.Jobs
{
    [PersistJobDataAfterExecution]
    [DisallowConcurrentExecution]
    public class GetWeatherDataJob : IJob
    {
        private readonly HttpClient _httpClient;
        private readonly IWeatherReportRepository _weatherReportRepository;
        private readonly ICityRepository _cityRepository;

        public GetWeatherDataJob(IHttpClientFactory httpClientFactory, IWeatherReportRepository weatherReportRepository, ICityRepository cityRepository)
        {
            _httpClient = httpClientFactory.CreateClient("WeatherApi");
            _weatherReportRepository = weatherReportRepository;
            _cityRepository = cityRepository;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var dataMap = context.JobDetail.JobDataMap;
            var initialized = dataMap.GetBoolean("initialized");

            var result = await _httpClient.GetAsync("https://api.openweathermap.org/data/2.5/group?id=2643743,3333169,6167865,6094817,361058,360630&appid=faa625de9ce05a0abdf9cf5850ca5637&units=metric");

            var apiWeatherWeatherReports = JsonConvert.DeserializeObject<Root>(await result.Content.ReadAsStringAsync());

            if (!initialized)
            {
                foreach (var w in apiWeatherWeatherReports.list)
                {
                    var city = new City
                    {
                        Id = w.id,
                        Name = w.name,
                        Country = w.sys.country
                    };

                    await _cityRepository.InsertIfNotExists(city);
                }

                dataMap.Put("initialized", true);
            }

            var weatherReports = new List<WeatherReport>();

            foreach (var w in apiWeatherWeatherReports.list)
            {
                var weatherReport = new WeatherReport
                {
                    CityId = w.id,
                    MinTemp = (int) w.main.temp_min,
                    MaxTemp = (int) w.main.temp_max,
                };

                weatherReports.Add(weatherReport);
            }

            _weatherReportRepository.InsertMany(weatherReports, context.FireTimeUtc.UtcDateTime);
        }
    }
}
