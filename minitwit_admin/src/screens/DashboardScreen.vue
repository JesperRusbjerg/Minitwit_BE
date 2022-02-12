<template>
  <div id="DashboardScreen" :style="{ backgroundColor: colors.darkGrey }">
    <twit-list-component :items="twitList" :height="'100%'" />
  </div>
</template>

<script>
import TwitListComponent from "@/components/TwitListComponent.vue";
import { useColors } from "vuestic-ui";
import { computed, inject } from "vue";

export default {
  name: "DashboardScreen",
  props: {},
  components: {
    TwitListComponent,
  },
  setup() {
    const store = inject("store");
    const { getColors } = useColors();
    const colors = computed(() => getColors());
    const twitList = computed(() => store.twits.state.twitList);
    
    const getTwitList = () => store.twits.actions.getTwitList();

    getTwitList();
    return {
      colors,
      twitList,
    };
  },
};
</script>

<style lang="scss" scoped>
#DashboardScreen {
  display: flex;
  justify-content: center;
}
</style>
