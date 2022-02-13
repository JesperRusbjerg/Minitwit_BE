import { readonly, reactive } from 'vue'
import usersApi from '@/api/users/users.js'

const state = reactive({
    loggedUser: 1,
})

const mutations = {
    loginUser(userId) {
        state.loggedUser = userId;
      },

    logoutUser() {
        state.loggedUser = 0
    }
}

const actions = {
    registerUser: async (userData) => {
        try {
            const res = await usersApi.registerUser(userData)
            const id = res.userId ? res.userId : 0;
            mutations.loginUser(id)
        } catch (e) {
            console.error(e)
        }
    },

    loginUser: async (userData) => {
        try {
            const res = await usersApi.loginUser(userData)
            const id = res ? res : 0;
            mutations.loginUser(id)
        } catch (e) {
            console.error(e)
        }
    }
}

export default {
    state: readonly(state),
    mutations: readonly(mutations),
    actions: readonly(actions)
}