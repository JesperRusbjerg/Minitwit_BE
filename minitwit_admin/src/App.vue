<template>
  <div id="app">
    <navbar :navbarColor="colors.black" :iconColor="colors.primaryColor" />
    <div class="app-content">
      <sidebar
        class="sidebar"
        :items="sidebarItems"
        :minimized="isSidebarMinimized"
        :color="colors.darkGrey"
        :iconColor="colors.primaryColor"
      />
      <router-view class="router-content" />
    </div>
  </div>
</template>

<script>
import Navbar from "@/components/Navbar.vue";
import Sidebar from "@/components/Sidebar.vue";
import { useColors } from "vuestic-ui";
import { ref, computed, provide } from "vue";
import store from '@/compositionStore/index'

export default {
  name: "App",
  components: {
    Navbar,
    Sidebar,
  },
  setup() {
    provide('store', store)
    const { getColors } = useColors();
    const colors = computed(() => getColors());
    const useSidebarItems = () => {
      return [{ title: "Dashboard", icon: "house", to: "/", active: true }];
    };
    const getSidebarItems = computed(() => useSidebarItems());
    const isSidebarMinimized = computed(() => store.state.isSidebarMinimized)
    return {
      colors,
      isSidebarMinimized,
      sidebarItems: getSidebarItems.value,
    };
  },
};
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

.app-content {
  display: flex;
  flex-direction: row;
}

.router-content {
  width: 100%;
}

.sidebar {
  height: 100vh;
}
</style>
