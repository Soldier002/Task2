using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.WeatherOverviewApi.ViewModels
{
    public class WeatherReportListViewModel
    {
        public string UtcNow { get; set; }

        public IList<WeatherReportViewModel> WeatherList { get; set; } = new List<WeatherReportViewModel>();
    }
}
