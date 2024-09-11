using Domain.Integration.Models;
using Domain.Persistence.Entities;
using Domain.Persistence.Repositories;
using Domain.Services.Services;
using Newtonsoft.Json;
using Quartz;
using Services.Services;

namespace Task2.Server.Jobs
{
    [PersistJobDataAfterExecution]
    [DisallowConcurrentExecution]
    public class GetWeatherDataJob : IJob
    {
        private readonly IGetWeatherDataService _getWeatherDataService;

        public GetWeatherDataJob(IGetWeatherDataService getWeatherDataService)
        {
            _getWeatherDataService = getWeatherDataService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await _getWeatherDataService.Execute(context.JobDetail.JobDataMap, context.FireTimeUtc.DateTime, context.CancellationToken);
        }
    }
}
