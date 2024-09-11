import { useEffect, useState } from "react";
import { getAllCitiesEndpoint, weatherOverviewApi } from "../../../services/endpoints";
import { CityViewModel } from '../../../models/city-viewmodel.model'
import { City } from '../../../models/city'

const mapCity = (cityViewModel: CityViewModel): City => {
    const colorHex = '#' + Math.floor(Math.random() * 16777215).toString(16)

    return {
        id: cityViewModel.id,
        name: cityViewModel.name,
        country: cityViewModel.country,
        color: colorHex
    }
}

export const useCities = () => {
    const [cities, setCities] = useState<City[]>([]);

    useEffect(() => {
        const fetchCities = async () => {
            const response = await fetch(`${weatherOverviewApi}${getAllCitiesEndpoint}`);
            const cityViewModels: CityViewModel[] = await response.json();
            const nextsCities = cityViewModels.map(mapCity)

            setCities(nextsCities);
        };

        fetchCities();
    }, []);

    return cities;
}