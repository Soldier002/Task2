## Launching instructions
- Open with Visual Studio 2022
- Make sure solution is configured to start multiple projects (Frontend & WeatherOverviewApi)
- Make sure user secrets for WeatherOverviewApi project look like this
```
{
  "ConnectionStrings": {
    "DefaultConnectionString": "" // sql server connection string
  },
  "WeatherApiKey": "" // https://home.openweathermap.org/api_keys
}
```
- Create database initial catalog point to (e.g. Task2)
- Enable ReadCommit transaction isolation for the db
```
ALTER DATABASE Task2
SET READ_COMMITTED_SNAPSHOT ON
GO
ALTER DATABASE Task2
SET ALLOW_SNAPSHOT_ISOLATION ON
GO
```

## Possible future developments
- Docker container support
- Integration tests for the whole app and persistence layer specifically
- Logging exceptions persistently
- Moving jobserver to a separate project
- Tests for frontend
