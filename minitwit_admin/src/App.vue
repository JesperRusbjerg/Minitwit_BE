<template>
  <navbar :navbarColor="colors.black" :iconColor="colors.primaryColor" />
  <sidebar :items="sidebarItems" :minimized="isSidebarMinimized" :color="colors.darkGrey" :iconColor="colors.primaryColor" />
  <router-view/>
</template>

<script>
import Navbar from "@/components/Navbar.vue";
import Sidebar from "@/components/Sidebar.vue";
import { useColors } from 'vuestic-ui'
import { ref, computed } from "vue"

export default {
  name: "App",
  components: {
    Navbar,
    Sidebar
  },
  setup() {
    const { getColors } = useColors()
      const colors = computed(() => getColors())
    const useSidebarItems = () => {
      return [{ title: "Dashboard", icon: "house", active: true }];
    };
    const getSidebarItems = computed(() => useSidebarItems());
    const isSidebarMinimized = ref(false)
    return {
      colors,
      isSidebarMinimized,
      sidebarItems: getSidebarItems.value
    }
  }
}
</script>

<style lang="scss">
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
}

#nav {
  padding: 30px;

  a {
    font-weight: bold;
    color: #2c3e50;

    &.router-link-exact-active {
      color: #42b983;
    }
  }
}
</style>
