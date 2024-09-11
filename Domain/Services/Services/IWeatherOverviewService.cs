using Domain.WeatherOverviewApi.ViewModels;

namespace Domain.Services.Services
{
    public interface IWeatherOverviewService
    {
        Task<WeatherReportListViewModel> ExecuteWeatherReportsSse();
    }
}