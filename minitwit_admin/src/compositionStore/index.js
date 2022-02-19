import { provide, inject, reactive, computed } from 'vue'
import sidebar from '@/compositionStore/sidebar/sidebarModule'
import async from '@/compositionStore/async/asyncModule'
import twits from '@/compositionStore/twits/twitsModule'
import users from '@/compositionStore/users/usersModule'
import followers from '@/compositionStore/followers/followersModule'

const initStore = () => {
    provide("useSidebar", sidebar)
    provide("useAsync", async)
    provide("useTwits", twits)
    provide("useUsers", users)
    provide("useFollowers", followers)
    provide("useSidebar", sidebar)
}

const useAsync = () => inject("useAsync")
const useTwits = () => inject("useTwits")
const useUsers = () => inject("useUsers")
const useFollowers = () => inject("useFollowers")
const useSidebar = () => inject("useSidebar")

export { 
    initStore,
    useSidebar,
    useAsync,
    useTwits,
    useUsers,
    useFollowers,
    async,
    twits,
    users,
    followers
}

export default {
    initStore,
    useSidebar,
    useAsync,
    useTwits,
    useUsers,
    useFollowers,
    async,
    twits,
    users,
    followers
}