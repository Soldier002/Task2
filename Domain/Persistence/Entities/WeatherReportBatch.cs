using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Persistence.Entities
{
    public class WeatherReportBatch
    {
        public long Id { get; set; }

        public DateTime CreationDateTime { get; set; }
    }
}
