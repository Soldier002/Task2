import { ChartData } from 'chart.js'

export interface LineChartProps {
    chartData: ChartData<"line">
    titleText: string
}