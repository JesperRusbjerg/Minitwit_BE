import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
import vuexStore from './store'
import compositionStore from './compositionStore/index'
import { VuesticPlugin } from 'vuestic-ui'
import 'vuestic-ui/dist/vuestic-ui.css'
import  colors from './utils/colors';

const app = createApp(App)
    .use(vuexStore)
    .use(router)
    .use(VuesticPlugin, {
        colors: {
            ...colors.darkTheme
        }
    })

app.provide("store", compositionStore)
app.mount('#app')