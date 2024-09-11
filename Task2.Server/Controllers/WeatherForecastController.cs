using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.IO.Pipelines;
using System.Net.Http;
using System.Text.Json.Serialization;

namespace Task2.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly HttpClient _httpClient;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient("WeatherApi");
        }

        [HttpGet("get")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);

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
            public string Country { get; set; }

            public string City { get; set; }

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
                // todo 404 etdc
                var result = await _httpClient.GetAsync("https://api.openweathermap.org/data/2.5/group?id=2643743,3333169,6167865,6094817,361058,360630&appid=faa625de9ce05a0abdf9cf5850ca5637&units=metric");

                var result2 = JsonConvert.DeserializeObject<Root>(await result.Content.ReadAsStringAsync());
                var rnd = new Random();
                var weatherListViewModel = new WeatherListViewModel();
                foreach (var w in result2.list)
                {
                    var wvm = new WeatherViewModel
                    {
                        Country = w.sys.country,
                        City = w.name,
                        MinTemp = rnd.NextInt64() % 20 + 10,//w.main.temp_min,
                        MaxTemp = w.main.temp_max,
                    };

                    weatherListViewModel.WeatherList.Add(wvm);
                }

                weatherListViewModel.UtcNow = DateTime.UtcNow;
                var camelSettings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
                await Response.WriteAsync("data: " + JsonConvert.SerializeObject(weatherListViewModel, camelSettings) + "\n\n");
                //Response.BodyWriter.AsStream();
                //await Response.WriteAsync(result.Content.ReadAsStream());
                await Response.Body.FlushAsync();
                await Task.Delay(5000);
            }
        }
    }
}
