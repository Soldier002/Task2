using Quartz;

namespace Domain.Services.Services
{
    public interface IGetWeatherDataService
    {
        Task Execute(JobDataMap jobDataMap, DateTime executionDateTime, CancellationToken ct);
    }
}