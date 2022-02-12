import { readonly, reactive } from 'vue'

const state = reactive({
    isLoading: false,
    error: undefined
})

const actions = {
    setLoading: (isLoading) => {
        state.isLoading = isLoading
    },

    setError: (error) => {
        state.error = error
    }
}

export default {
    state: readonly(state),
    actions: readonly(actions)
}