import { useEffect, Dispatch, SetStateAction } from 'react'
import { weatherOverviewApi, weatherSseEndpoint } from '../../../services/endpoints'
import { WeatherOverviewData } from '../../../models/weather-overview-data.model'
import { TempChartData } from '../../../models/temp-chart-data.model'
import { WeatherListViewModel } from '../../../models/weather-list-viewmodel.model'

export const useSseToUpdateWeatherOverviewData = (isInitialized: boolean, setWeatherOverviewData: Dispatch<SetStateAction<WeatherOverviewData>>) => {
    useEffect(() => {
        if (!isInitialized)
            return

        function updateChartsWith(nextWeather: WeatherListViewModel, prevChartData: TempChartData, minOrMaxTempGetterFunction: (weatherViewModel: WeatherViewModel) => number) {
            const nextChartData = { ...prevChartData }

            nextChartData.labels = [...prevChartData.labels, nextWeather.utcNow]

            nextChartData.datasets = prevChartData.datasets.map(ds => {
                const nextTemp = minOrMaxTempGetterFunction(nextWeather.weatherList.find(w => ds.cityId === w.cityId))
                const nextDs = {
                    ...ds,
                    data: [...ds.data, nextTemp]
                }

                return nextDs
            })

            return nextChartData
        }

        const es = new EventSource(`${weatherOverviewApi}${weatherSseEndpoint}`)
        es.onopen = () => console.log('SSE for Weather Reports opened')
        es.onerror = (e) => console.log('SSE for Weather Reports closed', e)
        es.onmessage = (e) => {
            const lastWeatherReport = JSON.parse(e.data)
            setWeatherOverviewData(oldChartData => (
                {
                    minTempChartData: updateChartsWith(lastWeatherReport, oldChartData.minTempChartData, (x) => x.minTemp),
                    maxTempChartData: updateChartsWith(lastWeatherReport, oldChartData.maxTempChartData, (x) => x.maxTemp)
                }
            ))
        }
        return () => es.close()
    }, [isInitialized, setWeatherOverviewData])
}