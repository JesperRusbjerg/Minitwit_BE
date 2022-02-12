import axios from 'axios'
import store from '@/compositionStore/index'

const apiURL = process.env.VUE_APP_ADMIN_API_URL

const apiRequest = (method, path, data, contentType) => {
    store.async.mutations.setLoading(true)
    const url = `${apiURL}${path}`;
    // TODO: ADD "x-api-key": store.getters.apiKey IF NEEDED LATER
    const headers = {
        "Content-Type": "application/json"
    }

    if  (contentType) {
        headers["Content-Type"] = contentType
    }

    return new Promise((resolve, reject) => {
        const axiosConfig = { method, url, data, headers }
        return axios(axiosConfig)
            .then((response) => {
                resolve(response.data)
                store.async.mutations.setLoading(false)
            })
            .catch((error) => {
                reject(error.response.data)
                store.async.mutations.setError(error.response.data)
            })
    })
}

export {
    apiRequest
}