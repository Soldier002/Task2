using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Persistence.Entities
{
    public class WeatherReport
    {
        public long Id { get; set; }

        public double MinTemp { get; set; }

        public double MaxTemp { get; set; }

        public long CityId { get; set; }

        public City City { get; set; }

        public long WeatherReportBatchId { get; set; }

        public WeatherReportBatch WeatherReportBatch { get; set; }
    }
}
