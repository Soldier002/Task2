import Chart from "chart.js/auto";
import { CategoryScale, ChartData, ChartDataset } from "chart.js";
import { LineChart } from "./components/LineChart"
import { useCities } from './hooks/use-cities.hook';
import { useWeatherReports } from './hooks/use-weather-reports.hook';
import { WeatherReport } from '../../models/weather-report'
import { City } from '../../models/city'

const mapToDatasets = (cities: City[], weatherReports: WeatherReport[], timeline: string[],
    minOrMaxtempGetter: (weatherReport: WeatherReport) => number): ChartData<"line"> => {
    const datasets: ChartDataset<"line">[] = cities.map(c => {
        return {
            label: `${c.name} [${c.country}]`,
            cityId: c.id,
            data: weatherReports.filter(x => x.cityId === c.id).map(minOrMaxtempGetter),
            borderColor: c.color,
            backgroundColor: c.color,
            borderWidth: 2
        }
    });

    return {
        labels: timeline,
        datasets
    }
}

export function WeatherOverview() {
    Chart.register(CategoryScale);

    const cities = useCities();
    const { weatherReports, timeline } = useWeatherReports();

    if (!cities || !weatherReports) {
        return (<div>Loading...</div>)
    }

    const minTempChartData = mapToDatasets(cities, weatherReports, timeline, ({ minTemp }) => minTemp);
    const maxTempChartData = mapToDatasets(cities, weatherReports, timeline, ({ maxTemp }) => maxTemp);

    return (
        <div>
            <LineChart chartData={minTempChartData} titleText={"Minimum Temperature"} />
            <LineChart chartData={maxTempChartData} titleText={"Maximum Temperature"} />
        </div>
    );

}