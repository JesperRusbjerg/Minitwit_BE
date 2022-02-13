import { createRouter, createWebHashHistory } from 'vue-router'

const DashboardScreen = () => import('@/screens/DashboardScreen.vue')
const UserEntranceScreen = () => import('@/screens/UserEntranceScreen.vue')
const UserProfileScreen = () => import('@/screens/UserProfileScreen.vue')

const routes = [
  {
    path: '/',
    name: 'MiniTwit Admin',
    component: DashboardScreen
  },
  {
    path: '/user-entrance',
    name: 'MiniTwit Login Page',
    component: UserEntranceScreen
  },
  {
    path: '/user-profile',
    name: 'MiniTwit User Page',
    component: UserProfileScreen
  },
]

const router = createRouter({
  history: createWebHashHistory(),
  routes
})

export default router
