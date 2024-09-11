using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Integration.Models
{
    public class OpenWeatherApiResponse
    {
        public IList<OpenWeather> list { get; set; }
    }

    public class OpenWeather
    {
        public Sys sys { get; set; }

        public string name { get; set; }

        public long id { get; set; }

        public Main main { get; set; }
    }

    public class Main
    {
        public double temp_min { get; set; }
        public double temp_max { get; set; }
    }

    public class Sys
    {
        public string country { get; set; }
    }

}
