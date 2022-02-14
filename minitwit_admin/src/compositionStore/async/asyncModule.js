import { readonly, reactive } from 'vue'

const state = reactive({
    isLoading: false,
    error: undefined
})

const actions = {
    setLoading: (isLoading) => {
        state.isLoading = isLoading
    },

    setError: (e) => {
        state.error = e
    }
}

export default {
    state: readonly(state),
    actions: readonly(actions)
}