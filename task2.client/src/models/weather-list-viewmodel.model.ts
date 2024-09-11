export interface WeatherListViewModel {
    utcNow: Date,
    weatherList: WeatherViewModel[]
}

interface WeatherViewModel {
    cityId: number,
    minTemp: number,
    maxTemp: number
}