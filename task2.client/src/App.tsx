import { useEffect, useState } from 'react';
import Chart from "chart.js/auto";
import { CategoryScale } from "chart.js";
import LineChart from "./LineChart"

import './App.css';


const defaultChartData = {
    labels: [],// Data.map((data) => data.year),
    datasets: [
        {
            label: "Cairo [EG]",
            data: [],
            borderColor: "red",
            backgroundColor: "red",
            borderWidth: 2
        },
        {
            label: "Alexandria [EG]",
            data: [],
            borderColor: "red",
            borderWidth: 2
        },
        {
            label: "London [GB]",
            data: [],
            borderColor: "green",
            borderWidth: 2
        },
        {
            label: "Manchester [GB]",
            data: [],
            borderColor: "green",
            borderWidth: 2
        },
        {
            label: "Ottawa [CA]",
            data: [],
            borderColor: "blue",
            borderWidth: 2
        },
        {
            label: "Toronto [CA]",
            data: [],
            borderColor: "blue",
            borderWidth: 2
        },
    ]
};

function App() {
    Chart.register(CategoryScale);

    const [chartData, setChartData] = useState(defaultChartData);

    useEffect(() => {
        function updateChartsWith(nextWeather, prevChartData) {
            let nextChartData = { ...prevChartData }

            nextChartData.labels = [...prevChartData.labels, nextWeather.utcNow]

            nextChartData.datasets = prevChartData.datasets.map(ds => {
                const nextMinTemp = nextWeather.weatherList.find(w => ds.label === `${w.city} [${w.country}]`).minTemp
                let nextDs = {
                    ...ds,
                    data: [...ds.data, nextMinTemp]
                }

                return nextDs
            })

            return nextChartData
        }

        const es = new EventSource('https://localhost:5173/weatherforecast/testSse')
        es.onopen = () => console.log('SSE opened')
        es.onerror = (e) => console.log('SSE error', e)
        es.onmessage = (e) => {

            console.log('SSES msg received')
            let data = JSON.parse(e.data)
            setChartData(c => updateChartsWith(data, c))
        }
        return () => es.close()
    }, [])




    useEffect(() => {
        populateWeatherData();
    }, []);

    const contents = <p><em>Loading... Please refresh once the ASP.NET backend has started. See <a href="https://aka.ms/jspsintegrationreact">https://aka.ms/jspsintegrationreact</a> for more details.</em></p>


    return (
        <div>
            <h1 id="tableLabel">Weather forecast</h1>
            <LineChart chartData={chartData} />
            {contents}
        </div>
    );

    async function populateWeatherData() {
        // const response = await fetch('weatherforecast/testSse');
        //const data = await response.json();
        //setForecasts(data);
    }
}

export default App;