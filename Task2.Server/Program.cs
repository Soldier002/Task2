using Domain.Persistence.Repositories;
using FluentMigrator.Runner;
using Persistence.Migrations;
using Persistence.Repositories;
using Polly;
using Quartz;
using System.Reflection;
using Task2.Server.Jobs;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient("WeatherApi")
    .AddTransientHttpErrorPolicy(policyBuilder =>
        policyBuilder.WaitAndRetryAsync(3, retryNumber => TimeSpan.FromMilliseconds(1000)));
builder.Services.AddTransient<GetWeatherDataJob>();
builder.Services.AddTransient<IWeatherReportRepository, WeatherReportRepository>();
builder.Services.AddTransient<ICityRepository, CityRepository>();

builder.Services.AddQuartz(q =>
{
    var jobKey = new JobKey(nameof(GetWeatherDataJob));
    q.AddJob<GetWeatherDataJob>(opts => opts
        .WithIdentity(jobKey)
        .StoreDurably()
        .UsingJobData("initialized", false));

    q.AddTrigger(opts => opts
        .ForJob(jobKey)
        .WithIdentity("GetWeatherDataJob-trigger")
        .WithCronSchedule("0/5 * * ? * * *")
    );
});
builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

builder.Configuration.AddUserSecrets(Assembly.GetExecutingAssembly());

var configuration = new ConfigurationBuilder()
    .AddUserSecrets(Assembly.GetExecutingAssembly())
    .Build();
var tmp = configuration.GetConnectionString("DefaultConnectionString");

builder.Services.AddFluentMigratorCore()
    .ConfigureRunner(rb => rb
        .AddSqlServer()
        .WithGlobalConnectionString(tmp)
        .ScanIn(typeof(_202409102330_AddCityAndWeatherTables).Assembly).For.Migrations());

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapFallbackToFile("/index.html");

using (var scope = app.Services.CreateScope())
{
    var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
    runner.MigrateUp();
}

app.Run();
