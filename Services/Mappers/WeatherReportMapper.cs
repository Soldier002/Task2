using Domain.Persistence.Entities;
using Domain.Services.Mappers;
using Domain.WeatherOverviewApi.ViewModels;

namespace Services.Mappers
{
    public class WeatherReportMapper : IWeatherReportMapper
    {
        public WeatherReportListViewModel ToWeatherReportListViewModel(IList<WeatherReport> weatherReports)
        {
            var weatherReportListViewModel = new WeatherReportListViewModel
            {
                UtcNow = weatherReports.First().WeatherReportBatch.CreationDateTime.ToString("HH:mm:ss")
            };

            foreach (var weatherReport in weatherReports)
            {
                var wvm = new WeatherReportViewModel
                {
                    CityId = weatherReport.CityId,
                    MinTemp = weatherReport.MinTemp,
                    MaxTemp = weatherReport.MaxTemp
                };

                weatherReportListViewModel.WeatherList.Add(wvm);
            }

            return weatherReportListViewModel;
        }
    }
}
