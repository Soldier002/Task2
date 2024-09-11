import { TempChartData } from '../models/temp-chart-data.model'

export interface WeatherOverviewData {
    minTempChartData: TempChartData,
    maxTempChartData: TempChartData
}