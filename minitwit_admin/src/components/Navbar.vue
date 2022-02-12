<template>
  <div id="Navbar">
    <va-navbar :color="navbarColor">
      <template v-slot:left>
        <div class="left">
          <va-icon
            @click="handleSidebarClick"
            :name="isSidebarMinimized ? 'close' : 'menu'"
            class="va-navbar__item"
            :color="iconColor"
          />
        </div>
      </template>
    </va-navbar>
  </div>
</template>

<script>
import { ref, computed } from 'vue'

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
      const sidebarState = ref(false)
      const isSidebarMinimized = computed(() => sidebarState.value)

      const handleSidebarClick = () => {
          sidebarState.value = !sidebarState.value
      }

      return {
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
