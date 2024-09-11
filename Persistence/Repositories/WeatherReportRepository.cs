using Dapper;
using Domain.Persistence.Entities;
using Domain.Persistence.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Persistence.Repositories
{
    public class WeatherReportRepository : IWeatherReportRepository
    {
        private readonly IConfiguration _configuration;

        public WeatherReportRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IList<WeatherReport>> GetAllFromLastBatch()
        {
            var sql = @"
				SELECT * 
				FROM WeatherReports wr
				JOIN WeatherReportBatches wrb ON wr.WeatherReportBatchId = wrb.Id
				WHERE wrb.Id IN (                
					SELECT TOP 1 Id
					FROM WeatherReportBatches wrb
					ORDER BY wrb.Id DESC)";

            using var connection = new SqlConnection(_configuration["DefaultConnectionString"]);
            var result = await connection.QueryAsync<WeatherReport, WeatherReportBatch, WeatherReport>(
                sql,
                map: (weatherReport, weatherReportBatch) =>
                {
                    weatherReport.WeatherReportBatch = weatherReportBatch;
                    return weatherReport;
                },
                splitOn: "Id");

            return result.ToList();
        }

        public async void InsertMany(IEnumerable<WeatherReport> weatherReports, DateTime creationDateTime)
        {
            var dataTable = MapFrom(weatherReports);

            using (var connection = new SqlConnection(_configuration["DefaultConnectionString"]))
            {
                //using var transaction = sourceConnection.BeginTransaction();

                connection.Open();
                var batchIdSql = @"
                    DECLARE @BatchIdTable TABLE (Id BIGINT)
                    DECLARE @BatchId BIGINT

                    INSERT INTO WeatherReportBatches 
                    OUTPUT INSERTED.Id INTO @BatchIdTable
                    VALUES (GETUTCDATE())

                    SELECT TOP 1 @BatchId = Id
                    FROM @BatchIdTable

                    SELECT @BatchId;";

                using var cmd = new SqlCommand(batchIdSql, connection);
                var batchId = (long)await cmd.ExecuteScalarAsync();

                foreach (DataRow row in dataTable.Rows)
                {
                    row["WeatherReportBatchId"] = batchId;
                }



                using (var bulkCopy = new SqlBulkCopy(
                           _configuration["DefaultConnectionString"], SqlBulkCopyOptions.CheckConstraints | SqlBulkCopyOptions.UseInternalTransaction))
                {
                    bulkCopy.BatchSize = sampleData.Count();
                    bulkCopy.DestinationTableName =
                        "dbo.WeatherReports";

                    bulkCopy.WriteToServer(dataTable);

                };
            }
        }
        List<WeatherReport> sampleData = new List<WeatherReport> {
                new WeatherReport {
                    MinTemp = 10,
                    MaxTemp = 20,
                    CityId = 1337
                },
                new WeatherReport {
                    MinTemp = 13,
                    MaxTemp = 23,
                    CityId = 1337
                },
                new WeatherReport {
                    MinTemp = 17,
                    MaxTemp = 27,
                    CityId = 1337,
                }
            };


        private DataTable MapFrom(IEnumerable<WeatherReport> reports)
        {
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("Id", typeof(long));
            dataTable.Columns.Add("MinTemp", typeof(int));
            dataTable.Columns.Add("MaxTemp", typeof(int));
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
