using Domain.Persistence.Entities;
using Domain.Persistence.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Persistence.Repositories;
using System.IO.Pipelines;
using System.Net.Http;
using System.Text.Json.Serialization;

namespace Task2.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherOverviewController : ControllerBase
    {
        private readonly ILogger<WeatherOverviewController> _logger;
        private readonly IWeatherReportRepository _weatherReportRepository;
        private readonly ICityRepository _cityRepository;
        private readonly HttpClient _httpClient;

        public WeatherOverviewController(ILogger<WeatherOverviewController> logger, IHttpClientFactory httpClientFactory, IWeatherReportRepository weatherReportRepository, ICityRepository cityRepository)
        {
            _logger = logger;
            _weatherReportRepository = weatherReportRepository;
            _cityRepository = cityRepository;
            _httpClient = httpClientFactory.CreateClient("WeatherApi");
        }

        [HttpGet("getAllCities")]
        public async Task<IEnumerable<City>> GetAllCities()
        {
            var cities = await _cityRepository.GetAll();

            return cities;
        }

        public class NotList
        {
            public Sys sys { get; set; }

            public string name { get; set; }

            public long id { get; set; }

            public Main main { get; set; }
        }

        public class Main
        {
            public double temp_min { get; set; }
            public double temp_max { get; set; }
        }

        public class Root
        {
            public List<NotList> list { get; set; }
        }

        public class Sys
        {
            public string country { get; set; }
        }

        public class WeatherListViewModel
        {
            public DateTime UtcNow { get; set; }

            public IList<WeatherViewModel> WeatherList { get; set; } = new List<WeatherViewModel>();
        }

        public class WeatherViewModel
        {
            public long CityId { get; set; }

            public double MinTemp { get; set; }

            public double MaxTemp { get; set; }
        }


        [HttpGet("testSse")]
        public async Task<string> TestSse()
        {
            Response.Headers.Add("Content-Type", "text/event-stream");
            Response.Headers.Add("Cache-Control", "no-cache");
            Response.Headers.Add("Connection", "keep-alive");
            while (true)
            {
                try
                {
                    var weatherReports = await _weatherReportRepository.GetAllFromLastBatch();

                    var rnd = new Random();
                    var vm = new WeatherListViewModel
                    {
                        UtcNow = weatherReports.First().WeatherReportBatch.CreationDateTime
                    };

                    foreach (var weatherReport in weatherReports)
                    {
                        var wvm = new WeatherViewModel
                        {
                            CityId = weatherReport.CityId,
                            MinTemp = weatherReport.MinTemp,
                            MaxTemp = weatherReport.MaxTemp
                        };

                        vm.WeatherList.Add(wvm);
                    }

                    var camelSettings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
                    await Response.WriteAsync("data: " + JsonConvert.SerializeObject(vm, camelSettings) + "\n\n");
                    //Response.BodyWriter.AsStream();
                    //await Response.WriteAsync(result.Content.ReadAsStream());
                    await Response.Body.FlushAsync();
                }
                catch (Exception ex)
                {
                    var ex2 = ex;
                }
                await Task.Delay(5000);
            }
        }
    }
}
