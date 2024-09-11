using Domain.Persistence.Entities;
using System.Data;

namespace Domain.Persistence.Mappers
{
    public interface IWeatherReportMapper
    {
        DataTable ToDataTable(IEnumerable<WeatherReport> reports);
    }
}