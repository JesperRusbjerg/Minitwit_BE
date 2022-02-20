import { reactive, computed } from "vue";

const state = reactive({
  isSidebarMinimized: false,
  selectedSidebarItem: "Dashboard"
});

const mutations = {
  toggleSidebar: () => {
    state.isSidebarMinimized = !state.isSidebarMinimized
  },

  setSelectedSidebarItem: (sidebarItem) => {
      state.selectedSidebarItem = sidebarItem
  }
};

const getSidebarMinimized = () => computed(() => state.isSidebarMinimized)
const getSelectedSidebarItem = () => computed(() => state.selectedSidebarItem)
const toggleSidebar = () => mutations.toggleSidebar()
const selectSidebar = (sidebarItem) => mutations.setSelectedSidebarItem(sidebarItem)

export {
    getSidebarMinimized,
    getSelectedSidebarItem,
    toggleSidebar,
    selectSidebar
}

export default {
    getSidebarMinimized,
    getSelectedSidebarItem,
    toggleSidebar,
    selectSidebar
}