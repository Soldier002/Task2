export interface WeatherReportListViewModel {
    utcNow: string
    weatherList: WeatherReportViewModel[]
}

export interface WeatherReportViewModel {
    cityId: number
    minTemp: number
    maxTemp: number
}