<template>
  <div id="Navbar">
    <va-navbar>
      <template v-slot:left>
        <div class="left">
          <va-icon
            @click="handleSidebarClick"
            :name="isSidebarMinimized ? 'close' : 'menu'"
            class="va-navbar__item"
            :color="colors.primaryColor"
          />
          <router-link to="/">
            <va-icon name="house" />
          </router-link>
        </div>
      </template>
    </va-navbar>
  </div>
</template>

<script>
import { useColors } from 'vuestic-ui'
import { ref, computed } from 'vue'

export default {
  name: "Navbar",
  props: {},
  components: {},
  setup() {
      const { getColors } = useColors()
      const colors = computed(() => getColors())

      const sidebarState = ref(false)

      const isSidebarMinimized = computed(() => sidebarState.value)

      const handleSidebarClick = () => {
          sidebarState.value = !sidebarState.value
      }

      return {
          colors,
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
