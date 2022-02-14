<template>
  <div id="Navbar">
    <va-navbar :color="navbarColor">
      <template v-slot:left>
        <div class="left">
          <va-icon
            @click="handleSidebarClick"
            :name="isSidebarMinimized ? 'menu' : 'close'"
            :color="iconColor"
          />
        </div>
      </template>
    </va-navbar>
  </div>
</template>

<script>
import { ref, computed, inject } from 'vue'

export default {
  name: "Navbar",
  props: {
      navbarColor: {
          type: String,
          required: false,
          default: '#AAAAAA'
      },
      iconColor: {
            type: String,
            required: false,
            default: '#FFF'
        },
  },
  components: {},
  setup() {
      const store = inject('store')
      const isSidebarMinimized = computed(() => store.state.isSidebarMinimized)

      const handleSidebarClick = () => store.actions.toggleSidebar()

      return {
          store,
          isSidebarMinimized,
          handleSidebarClick
      }
  }
};
</script>

<style lang="scss" scoped>
#Navbar {
    
}
</style>
