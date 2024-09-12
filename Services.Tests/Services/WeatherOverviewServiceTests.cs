using Domain.Persistence.Entities;
using Domain.Persistence.Repositories;
using Domain.Services.Mappers;
using Domain.WeatherOverviewApi.ViewModels;
using Moq;
using Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestsCommon.Extensions;

namespace Services.Tests.Services
{
    public class WeatherOverviewServiceTests
    {
        private Mock<IWeatherReportRepository> _weatherReportRepository;
        private Mock<IWeatherReportMapper> _weatherReportMapper;

        [OneTimeSetUp]
        public void SetUp()
        {
            _weatherReportRepository = MockUtils.Create<IWeatherReportRepository>();
            _weatherReportMapper = MockUtils.Create<IWeatherReportMapper>();
        }

        [Test]
        public async Task GivenHappyPath_WhenExecute_ReturnsData()
        {
            // arrange
            var ct = CancellationToken.None;
            IList<WeatherReport> weatherReports = new List<WeatherReport>();
            var weatheReportListViewModel = new WeatherReportListViewModel();
            _weatherReportRepository.Setup(x => x.GetAllFromLastBatch(ct)).Returns(Task.FromResult(weatherReports));
            _weatherReportMapper.Setup(x => x.ToWeatherReportListViewModel(weatherReports)).Returns(weatheReportListViewModel);
            var service = new WeatherOverviewService(_weatherReportRepository.Object, _weatherReportMapper.Object);

            // act
            var result = await service.ExecuteWeatherReportsSse(ct);

            // assert
            Assert.That(result, Is.EqualTo(weatheReportListViewModel));
        }
    }
}
