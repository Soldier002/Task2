using Domain.Persistence.Entities;

namespace Domain.Persistence.Repositories
{
    public interface IWeatherReportRepository
    {
        Task<IList<WeatherReport>> GetAllFromLastBatch();
        void InsertMany(IEnumerable<WeatherReport> weatherReports, DateTime creationDateTime);
    }
}