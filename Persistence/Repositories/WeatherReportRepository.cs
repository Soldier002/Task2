using Dapper;
using Domain.Common.Configuration;
using Domain.Persistence.Entities;
using Domain.Persistence.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Persistence.Repositories
{
    public class WeatherReportRepository : IWeatherReportRepository
    {
        private readonly IWeatherOverviewConfiguration _configuration;

        public WeatherReportRepository(IWeatherOverviewConfiguration configuration)
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

            using var connection = new SqlConnection(_configuration.DefaultConnectionString);
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

        public async Task InsertMany(IEnumerable<WeatherReport> weatherReports, DateTime creationDateTime, CancellationToken ct)
        {
            var dataTable = MapFrom(weatherReports);

            using var connection = new SqlConnection(_configuration.DefaultConnectionString);
            connection.Open();
            using var transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);
            try
            {
                var batchIdSql = @"
                    DECLARE @BatchIdTable TABLE (Id BIGINT)
                    DECLARE @BatchId BIGINT

                    INSERT INTO WeatherReportBatches 
                    OUTPUT INSERTED.Id INTO @BatchIdTable
                    VALUES (GETUTCDATE())

                    SELECT TOP 1 @BatchId = Id
                    FROM @BatchIdTable

                    SELECT @BatchId;";

                using var cmd = new SqlCommand(batchIdSql, connection, transaction);
                var batchId = (long)await cmd.ExecuteScalarAsync(ct);

                foreach (DataRow row in dataTable.Rows)
                {
                    row["WeatherReportBatchId"] = batchId;
                }

                using var bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.CheckConstraints, transaction);
                bulkCopy.BatchSize = weatherReports.Count();
                bulkCopy.DestinationTableName = "dbo.WeatherReports";
                ct.ThrowIfCancellationRequested();
                await bulkCopy.WriteToServerAsync(dataTable, ct);
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }

        private DataTable MapFrom(IEnumerable<WeatherReport> reports)
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
