import React from "react";
import { Line } from "react-chartjs-2";

export function LineChart({ chartData, titleText }) {
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
