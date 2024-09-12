using Domain.Integration.Models;
using Domain.Persistence.Entities;
using Domain.Persistence.Repositories;
using Domain.Services.Services;
using Newtonsoft.Json;
using Quartz;
using Services.Services;

namespace WeatherOverviewApi.Jobs
{
    [PersistJobDataAfterExecution]
    [DisallowConcurrentExecution]
    public class GetWeatherDataJob : IJob
    {
        private readonly IGetWeatherDataService _getWeatherDataService;
        private readonly ILogger<GetWeatherDataJob> _logger;

        public GetWeatherDataJob(IGetWeatherDataService getWeatherDataService, ILogger<GetWeatherDataJob> logger)
        {
            _getWeatherDataService = getWeatherDataService;
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                await _getWeatherDataService.Execute(context.JobDetail.JobDataMap, context.FireTimeUtc.DateTime, context.CancellationToken);
            } 
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Message}");
                
                var jobEx = new JobExecutionException(ex)
                {
                    RefireImmediately = false
                };

                throw jobEx;
            }
        }
    }
}
