<template>
  <div id="DashboardScreen">
    <twit-list-component :items="twitList" :height="'100%'" @onClick="handleOnTwitClick" @onFollowClick="handleOnFollowClick"/>
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
    
    const handleOnFollowClick = (userId) => {
      flagTwit(twit.messageId, twit.flagged)
    }

    getTwitList();
    return {
      colors,
      twitList,
      handleOnTwitClick,
      handleOnFollowClick
    };
  },
};
</script>

<style lang="scss" scoped>
#DashboardScreen {
  display: flex;
  justify-content: center;
  width: 100%;
}
</style>
