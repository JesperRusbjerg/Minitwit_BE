import { readonly, reactive } from 'vue'
import async from '@/compositionStore/async/asyncModule'

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
    actions: readonly(actions),
    async
}