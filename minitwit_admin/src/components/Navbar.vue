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
import { computed } from "vue"
import { useSidebar } from "@/compositionStore/index"

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
      const { getSidebarMinimized, toggleSidebar } = useSidebar()
      const isSidebarMinimized = computed(() => getSidebarMinimized.value)

      const handleSidebarClick = () =>  toggleSidebar()
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
