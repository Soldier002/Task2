import { weatherOverviewApi, getAllCitiesEndpoint } from './endpoints'

export async function getAllCities() {
    const response = await fetch(`${weatherOverviewApi}${getAllCitiesEndpoint}`)
    const responseBody = await response.json()

    return responseBody
}
