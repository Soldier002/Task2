using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Migrations
{
    [Migration(202409102330)]
    public class _202409102330_AddCityAndWeatherTables : Migration
    {
        public override void Up()
        {
            Create.Table("Cities")
                .WithColumn("Id").AsInt64().PrimaryKey()
                .WithColumn("Name").AsString().NotNullable()
                .WithColumn("Country").AsString().NotNullable();

            Create.Table("WeatherReportBatches")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("CreationDateTime").AsDateTime().NotNullable();

            Create.Table("WeatherReports")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("MinTemp").AsInt32().NotNullable()
                .WithColumn("MaxTemp").AsInt32().NotNullable()
                .WithColumn("CityId").AsInt64().NotNullable()
                .WithColumn("WeatherReportBatchId").AsInt64().NotNullable();

            Create.ForeignKey("FK_WeatherReports_Cities")
                .FromTable("WeatherReports").ForeignColumn("CityId")
                .ToTable("Cities").PrimaryColumn("Id");

            Create.ForeignKey("FK_WeatherReports_WeatherReportBatches")
                .FromTable("WeatherReports").ForeignColumn("WeatherReportBatchId")
                .ToTable("WeatherReportBatches").PrimaryColumn("Id");
        }

        public override void Down()
        {
            Delete.ForeignKey("FK_WeatherReports_Cities").OnTable("WeatherReports");
            Delete.ForeignKey("FK_WeatherReports_WeatherReportBatches").OnTable("WeatherReports");

            Delete.Table("Weather");
            Delete.Table("City");
        }
    }
}
