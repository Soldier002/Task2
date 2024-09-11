using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.WeatherOverviewApi.ViewModels
{
    public class WeatherReportViewModel
    {
        public long CityId { get; set; }

        public double MinTemp { get; set; }

        public double MaxTemp { get; set; }
    }
}
