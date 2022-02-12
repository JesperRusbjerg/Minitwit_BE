import { readonly, reactive } from 'vue'
import twitsApi from '@/api/twits/twits.js'

const state = reactive({
    twitList: []
})

const mutations = {
    setTwitList: (twitList) => {
        state.twitList = twitList
    },

    updateTwit: (messageId, flagged) => {
        const twitIndex = state.twitList.findIndex(twitItem =>
            twitItem.messageId == messageId
        )
        const newTwit = {
            ...state.twitList[twitIndex],
            flagged
        }

        state.twitList = [
            ...state.twitList.slice(0, twitIndex),
            newTwit,
            ...state.twitList.slice(twitIndex + 1)
        ]
    }
}

const actions = {
    getTwitList: async () => {
        try {
            const result = await twitsApi.fetchTwits()
            mutations.setTwitList(result)
        } catch (error) {
            console.log(error)
            throw error
        }
    },

    toggleFlag: async (messageId, flagged) => {
        try {
            await twitsApi.flagTwit({
                MessageId: messageId,
                FlagMessage: !flagged
            })
            mutations.updateTwit(messageId, !flagged)
        } catch (error) {
            console.log(error)
            throw error
        }
    }
}

export default {
    state: readonly(state),
    actions: readonly(actions)
}