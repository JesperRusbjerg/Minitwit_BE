<template>
  <div id="DashboardScreen" :style="{ backgroundColor: colors.darkGrey }">
    <twit-list-component :items="twitList" :height="'100%'" @onClick="handleOnTwitClick"/>
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
    const flagTwit = (messageId, flagged) => {
      store.twits.actions.toggleFlag(messageId, flagged)
    }
    const handleOnTwitClick = (twit) => {
      flagTwit(twit.messageId, twit.flagged)
    }

    getTwitList();
    return {
      colors,
      twitList,
      handleOnTwitClick
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
