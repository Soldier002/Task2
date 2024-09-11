using Domain.Persistence.Entities;
using Domain.Persistence.Mappers;
using System.Data;

namespace Persistence.Mappers
{
    public class WeatherReportMapper : IWeatherReportMapper
    {
        public DataTable ToDataTable(IEnumerable<WeatherReport> reports)
        {
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("Id", typeof(long));
            dataTable.Columns.Add("MinTemp", typeof(double));
            dataTable.Columns.Add("MaxTemp", typeof(double));
            dataTable.Columns.Add("CityId", typeof(long));
            dataTable.Columns.Add("WeatherReportBatchId", typeof(long));

            foreach (var report in reports)
            {
                DataRow row = dataTable.NewRow();
                row["MinTemp"] = report.MinTemp;
                row["MaxTemp"] = report.MaxTemp;
                row["CityId"] = report.CityId;
                dataTable.Rows.Add(row);
            }

            return dataTable;
        }
    }
}
