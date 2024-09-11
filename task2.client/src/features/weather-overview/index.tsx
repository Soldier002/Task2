import { useState } from 'react';
import Chart from "chart.js/auto";
import { CategoryScale } from "chart.js";
import { LineChart } from "./components/LineChart"
import { useFetchAllCities } from './hooks/useFetchAllCities'
import { useSseToUpdateWeatherOverviewData } from './hooks/useSseToUpdateWeatherOverviewData'
import { WeatherOverviewData } from '../../models/weather-overview-data.model'

export function WeatherOverview() {
    Chart.register(CategoryScale);

    const [weatherOverviewData, setWeatherOverviewData] = useState<WeatherOverviewData>();
    const isInitialized = !!weatherOverviewData

    useFetchAllCities({ setWeatherOverviewData })
    useSseToUpdateWeatherOverviewData(isInitialized, setWeatherOverviewData)


    if (!isInitialized) {
        return (<div>Loading...</div>)
    }

    return (
        <div>
            <LineChart chartData={weatherOverviewData.minTempChartData} titleText={"Minimum Temperature"} />
            <LineChart chartData={weatherOverviewData.maxTempChartData} titleText={"Maximum Temperature"} />
        </div>
    );

}