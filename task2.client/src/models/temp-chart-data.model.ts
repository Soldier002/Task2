import { ChartData } from 'chart.js'
export interface TempChartData extends ChartData<'line', number, unknown> {
    cityId: number
}