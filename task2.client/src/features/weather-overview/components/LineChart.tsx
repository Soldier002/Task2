import { Line } from "react-chartjs-2";
import { TempChartData } from '../../../models/temp-chart-data.model'

export function LineChart({ chartData, titleText }: { chartData: TempChartData, titleText: string })
{
    return (
        <div className="chart-container">
            <Line
                data={chartData}
                options={{
                    plugins: {
                        title: {
                            display: true,
                            text: titleText
                        },
                        legend: {
                            display: true
                        }
                    }
                }}
            />
        </div>
    );
}
