using Domain.Persistence.Entities;
using Domain.Persistence.Repositories;
using Domain.Services.Services;
using Domain.WeatherOverviewApi.ViewModels;
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
        private readonly ICityRepository _cityRepository;
        private readonly IWeatherOverviewService _weatherOverviewService;

        public WeatherOverviewController(ICityRepository cityRepository, IWeatherOverviewService weatherOverviewService)
        {
            _cityRepository = cityRepository;
            _weatherOverviewService = weatherOverviewService;
        }

        [HttpGet("getAllCities")]
        public async Task<IEnumerable<City>> GetAllCities()
        {
            var cities = await _cityRepository.GetAll();

            return cities;
        }

        [HttpGet("weatherReportsSse")]
        public async Task WeatherReportsSse()
        {
            Response.Headers.Add("Content-Type", "text/event-stream");
            Response.Headers.Add("Cache-Control", "no-cache");
            Response.Headers.Add("Connection", "keep-alive");
            while (true)
            {
                var weatherReportListViewModel = await _weatherOverviewService.ExecuteWeatherReportsSse();

                var camelSettings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
                await Response.WriteAsync("data: " + JsonConvert.SerializeObject(weatherReportListViewModel, camelSettings) + "\n\n");
                await Response.Body.FlushAsync();

                await Task.Delay(5000);
            }
        }
    }
}
