using Domain.Persistence.Repositories;
using Domain.Services.Mappers;
using Domain.Services.Services;
using Domain.WeatherOverviewApi.ViewModels;

namespace Services.Services
{
    public class WeatherOverviewService : IWeatherOverviewService
    {
        private readonly IWeatherReportRepository _weatherReportRepository;
        private readonly IWeatherReportMapper _weatherReportMapper;

        public WeatherOverviewService(IWeatherReportRepository weatherReportRepository, IWeatherReportMapper weatherReportMapper)
        {
            _weatherReportRepository = weatherReportRepository;
            _weatherReportMapper = weatherReportMapper;
        }

        public async Task<WeatherReportListViewModel> ExecuteWeatherReportsSse()
        {
            var weatherReports = await _weatherReportRepository.GetAllFromLastBatch();
            var weatherReportListViewModel = _weatherReportMapper.ToWeatherReportListViewModel(weatherReports);

            return weatherReportListViewModel;
        }
    }
}
