using Domain.Persistence.Entities;
using Domain.WeatherOverviewApi.ViewModels;

namespace Domain.Services.Mappers
{
    public interface IWeatherReportMapper
    {
        WeatherReportListViewModel ToWeatherReportListViewModel(IList<WeatherReport> weatherReports);
    }
}