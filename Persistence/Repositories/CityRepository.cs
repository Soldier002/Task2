using Dapper;
using Domain.Persistence.Entities;
using Domain.Persistence.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Persistence.Repositories
{
    public class CityRepository : ICityRepository
    {
        private readonly IConfiguration _configuration;

        public CityRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task InsertIfNotExists(City city)
        {
            var sql = @"
                MERGE INTO Cities AS Target
                USING (VALUES (@Id, @Name, @Country)) AS Source (Id, Name, Country)
                ON Target.Id = Source.Id
                WHEN NOT MATCHED BY Target THEN
                    INSERT (Id, Name, Country)
                    VALUES (Source.Id, Source.Name, Source.Country);";

            using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnectionString"));
            await connection.ExecuteAsync(sql, city);
        }

        public async Task<IList<City>> GetAll()
        {
            var sql = "SELECT * FROM Cities";

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnectionString")))
            {
                var cities = await connection.QueryAsync<City>(sql);
                return cities.ToList();
            }
    }
}
}
