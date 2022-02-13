import { readonly, reactive } from 'vue'
import async from '@/compositionStore/async/asyncModule'
import twits from '@/compositionStore/twits/twitsModule'
import users from '@/compositionStore/users/usersModule'
import followers from '@/compositionStore/followers/followersModule'

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
    async,
    twits,
    users,
    followers
}