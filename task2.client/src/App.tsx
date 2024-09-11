import { WeatherOverview } from "./features/weather-overview/index"
import './App.css';

function App() {

    return (
        <div>
            <h1 id="tableLabel">Weather Report History</h1>
            <WeatherOverview />
        </div>
    );
}

export default App;