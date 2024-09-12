using Domain.Integration.ApiClients;
using Domain.Integration.Models;
using Domain.Persistence.Entities;
using Domain.Persistence.Repositories;
using Domain.Services.Mappers;
using Moq;
using Quartz;
using Services.Services;
using System.Data;
using TestsCommon.Extensions;
using IWeatherReportMapper = Domain.Persistence.Mappers.IWeatherReportMapper;

namespace Services.Tests.Services
{
    public class GetWeatherDataServiceTests
    {
        private Mock<JobDataMap> _jobDataMap;
        private Mock<IOpenWeatherMapApiClient> _openWeatherMapApiClient;
        private Mock<IOpenWeatherMapApiResponseMapper> _openWeatherMapApiResponseMapper;
        private Mock<ICityRepository> _cityRepository;
        private Mock<IWeatherReportMapper> _weatherReportMapper;
        private Mock<IWeatherReportRepository> _weatherReportRepository;

        [OneTimeSetUp]
        public void SetUp()
        {
            _jobDataMap = MockUtils.Create<JobDataMap>();
            _openWeatherMapApiClient = MockUtils.Create<IOpenWeatherMapApiClient>();
            _openWeatherMapApiResponseMapper = MockUtils.Create<IOpenWeatherMapApiResponseMapper>();
            _cityRepository = MockUtils.Create<ICityRepository>();
            _weatherReportMapper = MockUtils.Create<IWeatherReportMapper>();
            _weatherReportRepository = MockUtils.Create<IWeatherReportRepository>();
        }

        [Test]
        public async Task GivenHappyPathAndNotInitialized_WhenExecute_SuccessAndInitialized()
        {
            // arrange
            var initializedName = "initialized";
            var now = DateTime.UtcNow;
            var ct = CancellationToken.None;
            var openWeatherMapApiResponse = new OpenWeatherApiResponse
            {
                list = new List<OpenWeather>() { new() }
            };
            var city = new City();
            var weatherReports = new List<WeatherReport>();
            var dataTable = new DataTable();

            _jobDataMap.Setup(x => x.GetBoolean(initializedName)).Returns(false);
            _jobDataMap.Setup(x => x.Put(initializedName, true));
            _openWeatherMapApiClient.Setup(x => x.GetWeatherInAllCities(ct)).Returns(Task.FromResult(openWeatherMapApiResponse));
            _openWeatherMapApiResponseMapper.Setup(x => x.ToCity(openWeatherMapApiResponse.list.First())).Returns(city);
            _cityRepository.Setup(x => x.InsertIfNotExists(city, ct)).Returns(Task.CompletedTask);
            _openWeatherMapApiResponseMapper.Setup(x => x.ToWeatherReports(openWeatherMapApiResponse)).Returns(weatherReports);
            _weatherReportMapper.Setup(x => x.ToDataTable(weatherReports)).Returns(dataTable);
            _weatherReportRepository.Setup(x => x.InsertMany(dataTable, now, ct)).Returns(Task.CompletedTask);

            var service = new GetWeatherDataService(_openWeatherMapApiClient.Object, _weatherReportRepository.Object, _cityRepository.Object, _openWeatherMapApiResponseMapper.Object, _weatherReportMapper.Object);

            // act
            await service.Execute(_jobDataMap.Object, now, ct);

            // assert
            _cityRepository.Verify(x => x.InsertIfNotExists(city, ct), Times.Once);
            _jobDataMap.Verify(x => x.Put(initializedName, true), Times.Once);
            _weatherReportRepository.Verify(x => x.InsertMany(dataTable, now, ct), Times.Once);
        }

        [Test]
        public async Task GivenHappyPathAndInitialized_WhenExecute_SuccessAndNoInitialization()
        {
            // arrange
            var initializedName = "initialized";
            var now = DateTime.UtcNow;
            var ct = CancellationToken.None;
            var openWeatherMapApiResponse = new OpenWeatherApiResponse
            {
                list = new List<OpenWeather>() { new() }
            };
            var city = new City();
            var weatherReports = new List<WeatherReport>();
            var dataTable = new DataTable();

            _jobDataMap.Setup(x => x.GetBoolean(initializedName)).Returns(true);
            _openWeatherMapApiClient.Setup(x => x.GetWeatherInAllCities(ct)).Returns(Task.FromResult(openWeatherMapApiResponse));
            _openWeatherMapApiResponseMapper.Setup(x => x.ToWeatherReports(openWeatherMapApiResponse)).Returns(weatherReports);
            _weatherReportMapper.Setup(x => x.ToDataTable(weatherReports)).Returns(dataTable);
            _weatherReportRepository.Setup(x => x.InsertMany(dataTable, now, ct)).Returns(Task.CompletedTask);

            var service = new GetWeatherDataService(_openWeatherMapApiClient.Object, _weatherReportRepository.Object, _cityRepository.Object, _openWeatherMapApiResponseMapper.Object, _weatherReportMapper.Object);

            // act
            await service.Execute(_jobDataMap.Object, now, ct);

            // assert
            _cityRepository.Verify(x => x.InsertIfNotExists(city, ct), Times.Never);
            _jobDataMap.Verify(x => x.Put(initializedName, true), Times.Never);
            _weatherReportRepository.Verify(x => x.InsertMany(dataTable, now, ct), Times.Once);
        }
    }
}
