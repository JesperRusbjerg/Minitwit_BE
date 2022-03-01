<template>
  <div id="FollowersComponent">
    <va-list>
      <va-list-item class="follower-card" v-for="(item, index) in followers" :key="index">
        <va-list-item-section>
          <va-list-item-label>
            User ID: {{ item.whomId }}
          </va-list-item-label>
        </va-list-item-section>
        <va-list-item-section icon>
          <div class="submit-btn">
                <input type="submit" value="Unfollow" @click="handleItemClick(item)">
            </div>
        </va-list-item-section>
      </va-list-item>
    </va-list>
  </div>
</template>

<script>
import { computed } from "vue";

export default {
  name: "FollowersComponent",
  props: {
    items: {
      type: Array,
      required: true,
      default: []
    },
  },
  components: {},
  emits: ["onClick"],
  setup(props, context) {
    const followers = computed(() => props.items);
    const handleItemClick = (item) => {
      context.emit("onClick", item);
    };

    return {
      followers,
      handleItemClick,
    };
  },
};
</script>

<style lang="scss" scoped>
@import '../_variables.scss';

#FollowersComponent {
  width: fit-content;

  .va-list {
    padding: 0 !important;
    border-top: 1px solid $border-background;

  }
}
</style>
