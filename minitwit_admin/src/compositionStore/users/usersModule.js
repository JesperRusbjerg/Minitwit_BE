import { readonly, reactive } from 'vue'
import usersApi from '@/api/users/users.js'

const state = reactive({
    loggedUser: 0,
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
            const res = await usersApi.registerUser(userData);
            mutations.loginUser(res);
        } catch (e) {
            console.error(e)
        }
    },

    loginUser: async (userData) => {
        try {
            const id = await usersApi.loginUser(userData)
            if (id == 0) {
                throw Error("Encountered errors while registering the user.")
            } else {
                mutations.loginUser(id)
            }
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