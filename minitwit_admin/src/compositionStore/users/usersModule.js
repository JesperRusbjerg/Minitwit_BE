import { readonly, reactive, computed } from 'vue'
import store from '@/compositionStore/index'
import usersApi from '@/api/users/users.js'

const state = reactive({
    loggedUser: 0,
})

const mutations = {
    loginUser(userId) {
        state.loggedUser = userId;
        localStorage.setItem('loggedUser', userId)
    },
    
    logoutUser() {
        state.loggedUser = 0
        localStorage.setItem('loggedUser', 0)
    }
}

const actions = {
    registerUser: async (userData) => {
        try {
            const res = await usersApi.registerUser(userData);
            mutations.loginUser(res);
        } catch (e) {
            console.error(e)
        }
    },

    loginUser: async (userData) => {
        try {
            const id = await usersApi.loginUser(userData)
            if (!id || id == 0) {
                store.async.actions.setError("Encountered errors while logging the user.")
            } else {
                mutations.loginUser(id)
            }
        } catch (e) {
            console.error(e)
            return e.response.data;
        }
    }
}

const enforceLoggedUser = (id) => mutations.loginUser(id)
const getLoggedInUser = () => computed(() => state.loggedUser)
const logoutUser = () => mutations.logoutUser()

export {
    getLoggedInUser,
    logoutUser,
    enforceLoggedUser
}

export default {
    state: readonly(state),
    mutations: readonly(mutations),
    actions: readonly(actions),
    getLoggedInUser,
    logoutUser,
    enforceLoggedUser
}