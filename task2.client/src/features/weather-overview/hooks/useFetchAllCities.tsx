import { useState, useEffect } from "react";
import { getAllCities } from '../../../services/citiesApiClient'

export const useFetchAllCities = (setWeatherOverviewData) => {
    useEffect(() => {
        const fetchData = async () => {
            const cities = await getAllCities()
            const minTempChartData = {
                labels: [],
                datasets: cities.map(c => {
                    const colorHex = '#' + Math.floor(Math.random() * 16777215).toString(16)
                    return {
                        label: `${c.name} [${c.country}]`,
                        cityId: c.id,
                        data: [],
                        borderColor: colorHex,
                        backgroundColor: colorHex,
                        borderWidth: 2
                    }
                })
            }
            const maxTempChartData = {
                ...minTempChartData,
                datasets: minTempChartData.datasets.map(ds => ({ ...ds, data: [] }))
            }

            const weatherOverviewData = {
                minTempChartData,
                maxTempChartData
            }

            setWeatherOverviewData((c) => weatherOverviewData)
        }
        fetchData().catch(console.error)
    }, [])
}