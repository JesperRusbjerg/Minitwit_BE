<template>
  <div id="Sidebar">
    <va-sidebar
      :width="width"
      :minimized="minimized"
      :minimizedWidth="minimizedWidth"
    >
      <template v-for="(item, index) in items" :key="index">
        <div v-show="item.visible == 'always' ? true : item.visible.value">
          <va-sidebar-item
            :to="item.to"
            :active="isSelectedSidebarItem(item.title)"
            :active-color="color"
            @click="handleItemClick(item)"
          >
            <va-sidebar-item-content>
              <va-icon :name="item.icon" :color="iconColor" />
              <va-sidebar-item-title
                v-if="!minimized"
                :style="`height: ${itemTitleHeight}`"
              >
                {{ item.title }}
              </va-sidebar-item-title>
            </va-sidebar-item-content>
          </va-sidebar-item>
        </div>
      </template>
    </va-sidebar>
  </div>
</template>

<script>
import { computed } from "vue"
import { useSidebar } from "@/compositionStore/index"

export default {
  name: "Sidebar",
  props: {
    items: {
      type: Array,
      required: true,
      default: [],
    },
    color: {
      type: String,
      required: false,
      default: "#AAAAAA",
    },
    iconColor: {
      type: String,
      required: false,
      default: "#FFF",
    },
    width: {
      type: String,
      default: "16rem",
    },
    minimized: {
      type: Boolean,
      required: true,
    },
    minimizedWidth: {
      type: String,
      required: false,
      default: "4.5rem",
    },
    itemTitleHeight: {
      type: String,
      required: false,
      default: undefined,
    },
  },
  components: {},
  emits: ["onItemClick"],
  setup(props, context) {
    const { getSelectedSidebarItem } = useSidebar()
    const selectedSidebarItem = computed(() => getSelectedSidebarItem.value);

    const isSelectedSidebarItem = (sidebarItem) =>
      sidebarItem == selectedSidebarItem.value;

    const handleItemClick = (item) => {
      context.emit("onItemClick", item);
    };
    return {
      handleItemClick,
      isSelectedSidebarItem
    };
  },
};
</script>

<style lang="scss" scoped>
#Sidebar {
  
}
</style>
