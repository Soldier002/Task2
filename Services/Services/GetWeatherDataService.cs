using Domain.Integration.ApiClients;
using Domain.Persistence.Mappers;
using Domain.Persistence.Repositories;
using Domain.Services.Mappers;
using Domain.Services.Services;
using Quartz;
using IWeatherReportMapper = Domain.Persistence.Mappers.IWeatherReportMapper;

namespace Services.Services
{
    public class GetWeatherDataService : IGetWeatherDataService
    {
        private readonly IOpenWeatherMapApiClient _openWeatherMapApiClient;
        private readonly IWeatherReportRepository _weatherReportRepository;
        private readonly ICityRepository _cityRepository;
        private readonly IOpenWeatherMapApiResponseMapper _openWeatherMapApiResponseMapper;
        private readonly IWeatherReportMapper _weatherReportMapper;

        public GetWeatherDataService(IOpenWeatherMapApiClient openWeatherMapApiClient, IWeatherReportRepository weatherReportRepository, ICityRepository cityRepository, IOpenWeatherMapApiResponseMapper openWeatherMapApiResponseMapper, IWeatherReportMapper weatherReportMapper)
        {
            _openWeatherMapApiClient = openWeatherMapApiClient;
            _weatherReportRepository = weatherReportRepository;
            _cityRepository = cityRepository;
            _openWeatherMapApiResponseMapper = openWeatherMapApiResponseMapper;
            _weatherReportMapper = weatherReportMapper;
        }

        public async Task Execute(JobDataMap jobDataMap, DateTime executionDateTime, CancellationToken ct)
        {
            bool initialized;
            initialized = jobDataMap.GetBoolean(nameof(initialized));

            var openWeatherMapApiResponse = await _openWeatherMapApiClient.GetWeatherInAllCities(ct);

            if (!initialized)
            {
                foreach (var w in openWeatherMapApiResponse.list)
                {
                    var city = _openWeatherMapApiResponseMapper.ToCity(w);
                    await _cityRepository.InsertIfNotExists(city, ct);
                }

                jobDataMap.Put(nameof(initialized), true);
            }

            var weatherReports = _openWeatherMapApiResponseMapper.ToWeatherReports(openWeatherMapApiResponse);
            var weatherReportDataTable = _weatherReportMapper.ToDataTable(weatherReports);
            await _weatherReportRepository.InsertMany(weatherReportDataTable, executionDateTime, ct);
        }
    }
}
