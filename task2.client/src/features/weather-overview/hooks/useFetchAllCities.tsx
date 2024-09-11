import { useEffect, Dispatch, SetStateAction } from "react";
import { getAllCities } from '../../../services/citiesApiClient'
import { WeatherOverviewData } from '../../../models/weather-overview-data.model'
import { City } from '../../../models/city.model'
import { TempChartData } from '../../../models/temp-chart-data.model'

export const useFetchAllCities = (setWeatherOverviewData: Dispatch<SetStateAction<WeatherOverviewData>>) => {
    useEffect(() => {
        const fetchData = async () => {
            const cities: City[] = await getAllCities()
            const minTempChartData: TempChartData = {
                labels: [],
                datasets: cities.map(c => {
                    const colorHex = '#' + Math.floor(Math.random() * 16777215).toString(16)
                    return {
                        label: `${c.name} [${c.country}]`,
                        cityId: c.id,
                        data: [],
                        borderColor: colorHex,
                        backgroundColor: colorHex,
                        borderWidth: 2
                    }
                }) 
            }
            const maxTempChartData = {
                ...minTempChartData,
                datasets: minTempChartData.datasets.map(ds => ({ ...ds, data: [] }))
            }

            const weatherOverviewData = {
                minTempChartData,
                maxTempChartData
            }

            setWeatherOverviewData(() => weatherOverviewData)
        }
        fetchData().catch(console.error)
    }, [])
}