import { readonly, reactive } from 'vue'

const state = reactive({
    isSidebarMinimized: false
})

const actions = {
    toggleSidebar: () => {
        state.isSidebarMinimized = !state.isSidebarMinimized
    }
}

export default {
    state: readonly(state),
    actions: readonly(actions)
}