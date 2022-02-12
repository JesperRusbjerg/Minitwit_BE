import { readonly, reactive } from 'vue'

const state = reactive({
    isLoading: false,
    error: undefined
})

const mutations = {
    setLoading: (isLoading) => {
        state.isLoading = isLoading
    },

    setError: (error) => {
        state.error = error
    }
}

export default {
    state: readonly(state),
    mutations: readonly(mutations)
}