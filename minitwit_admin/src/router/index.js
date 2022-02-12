import { createRouter, createWebHashHistory } from 'vue-router'

const DashboardScreen = () => import('@/screens/DashboardScreen.vue')

const routes = [
  {
    path: '/',
    name: 'MiniTwit Admin',
    component: DashboardScreen
  }
]

const router = createRouter({
  history: createWebHashHistory(),
  routes
})

export default router
