using FluentMigrator;

namespace Persistence.Migrations
{
    [Migration(202409120059)]
    public class _202409120059_ChangeTempTypeFromIntToDouble : Migration
    {
        public override void Up()
        {
            Alter.Column("MinTemp")
                .OnTable("WeatherReports")
                .AsDouble();

            Alter.Column("MaxTemp")
                .OnTable("WeatherReports")
                .AsDouble();
        }

        public override void Down()
        {
            Alter.Column("MinTemp")
                .OnTable("WeatherReports")
                .AsInt32();

            Alter.Column("MaxTemp")
                .OnTable("WeatherReports")
                .AsInt32();
        }
    }
}
