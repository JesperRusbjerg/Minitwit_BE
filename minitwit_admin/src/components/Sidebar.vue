<template>
  <div id="Sidebar">
    <va-sidebar
      :width="width"
      :minimized="minimized"
      :minimizedWidth="minimizedWidth"
    >
      <template v-for="(item, index) in items" :key="index">
        <va-sidebar-item :to="item.to" :active="item.active" :active-color="color">
          <va-sidebar-item-content>
            <va-icon :name="item.icon" :color="iconColor" />
            <va-sidebar-item-title v-if="!minimized" :style="`height: ${itemTitleHeight}`">
              {{ item.title }}
            </va-sidebar-item-title>
          </va-sidebar-item-content>
        </va-sidebar-item>
      </template>
    </va-sidebar>
  </div>
</template>

<script>
export default {
  name: "Sidebar",
  props: {
      items: {
          type: Array,
          required: true,
          default: []
      },
      color: {
            type: String,
            required: false,
            default: '#AAAAAA'
        },
        iconColor: {
            type: String,
            required: false,
            default: '#FFF'
        },
      width: {
          type: String,
          default: '16rem'
      },
      minimized: {
          type: Boolean,
          required: true
      },
      minimizedWidth: {
          type: String,
          required: false,
          default: '4.5rem'
      },
      itemTitleHeight: {
          type: String,
          required: false,
          default: undefined
      }
  },
  components: {},
  setup() {
    return {};
  },
};
</script>

<style lang="scss" scoped>
#Sidebar {
    .va-icon {
        position: relative;
        float: left !important;
    }
}
</style>
