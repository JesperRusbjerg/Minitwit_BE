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
        @onItemClick="handleSidebarItemClick"
      />
      <router-view class="router-content" />
    </div>
  </div>
</template>

<script>
import Navbar from "@/components/Navbar.vue";
import Sidebar from "@/components/Sidebar.vue";
import { useColors } from "vuestic-ui";
import { computed } from "vue";
import { initStore } from "@/compositionStore/index";
import {
  getSidebarMinimized,
  selectSidebar,
} from "@/compositionStore/sidebar/sidebarModule";
import {
  enforceLoggedUser,
  logoutUser,
} from "@/compositionStore/users/usersModule";

export default {
  name: "App",
  components: {
    Navbar,
    Sidebar,
  },
  setup() {
    initStore();
    const { getColors } = useColors();
    const colors = computed(() => getColors());
    const storedUser = localStorage.getItem('loggedUser')

    if (!!storedUser && storedUser != 0) {
       enforceLoggedUser(storedUser);
    }
    
    const useSidebarItems = () => {
      return [
        {
          title: "Dashboard",
          to: "/",
          visibleToLoggedUser: "always",
          function: (title) => selectSidebar(title),
        },
        {
          title: "Login or register",
          to: "user-entrance",
          visibleToLoggedUser: false,
          function: (title) => selectSidebar(title),
        },
        {
          title: "User profile/create twit",
          to: "/user-profile",
          visibleToLoggedUser: true,
          function: (title) => selectSidebar(title),
        },
        {
          title: "Logout",
          to: "/",
          visibleToLoggedUser: true,
          function: (title) => handleLogoutUser(title),
        },
      ];
    };
    const getSidebarItems = computed(() => useSidebarItems());

    const handleSidebarItemClick = (item) => {
      item.function(item.title);
    };

    const handleLogoutUser = () => {
      selectSidebar("Dashboard");
      logoutUser();
    };

    return {
      colors,
      isSidebarMinimized: getSidebarMinimized(),
      sidebarItems: getSidebarItems.value,
      handleSidebarItemClick,
    };
  },
};
</script>

<style lang="scss">
@import "./genericStyles.scss"; 

#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  height: 100vh;
  width: 100vw;
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
  width: 100%;
  height: 100%;
  background-image: url('./assets/svgs/background.svg');
  background-size: cover;
}

.router-content {
  overflow: scroll;
}
</style>
