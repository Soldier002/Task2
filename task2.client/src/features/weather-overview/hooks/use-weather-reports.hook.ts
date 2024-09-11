import { useEffect, useState } from "react";
import { weatherOverviewApi, weatherSseEndpoint } from "../../../services/endpoints";
import { WeatherReport } from '../../../models/weather-report'
import { WeatherReportViewModel, WeatherReportListViewModel } from '../../../models/weather-list-viewmodel.model'


const mapWeatherReport = (data: WeatherReportViewModel): WeatherReport => {
    return {
        cityId: data.cityId,
        minTemp: data.minTemp,
        maxTemp: data.maxTemp,
    }
}

export const useWeatherReports = () => {
    const [weatherReports, setWeatherReports] = useState<WeatherReport[]>([]);
    const [timeline, setTimeline] = useState<string[]>([]);

    useEffect(() => {
        const es = new EventSource(`${weatherOverviewApi}${weatherSseEndpoint}`);
        es.onopen = () => console.log('SSE for Weather Reports opened')
        es.onerror = (e) => console.log('SSE for Weather Reports closed', e)
        es.onmessage = (e) => {
            const weatherReportListViewModel: WeatherReportListViewModel = JSON.parse(e.data)
            const newReport = weatherReportListViewModel.weatherList.map((w) => mapWeatherReport(w))
            setWeatherReports((prevReports) => [...prevReports, ...newReport]);
            setTimeline((prevTimeline) => [...prevTimeline, weatherReportListViewModel.utcNow])
        }

        return () => es.close();
    }, []);

    return { weatherReports, timeline };
}