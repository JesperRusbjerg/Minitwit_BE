import { readonly, reactive } from 'vue'
import twitsApi from '@/api/twits/twits.js'

const state = reactive({
    twitList: []
})

const actions = {
    setTwitList: (twitList) => {
        state.twitList = twitList
    },

    toggleFlag: (messageId, flagAction) => {
        twitsApi.flagTwit({
            messageId,
            flagAction
        })
    }
}

export default {
    state: readonly(state),
    actions: readonly(actions)
}