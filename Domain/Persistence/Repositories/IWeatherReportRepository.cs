using Domain.Persistence.Entities;
using System.Data;

namespace Domain.Persistence.Repositories
{
    public interface IWeatherReportRepository
    {
        Task<IList<WeatherReport>> GetAllFromLastBatch(CancellationToken ct);
        Task InsertMany(DataTable weatherReports, DateTime creationDateTime, CancellationToken ct);
    }
}