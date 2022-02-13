<template>
  <div id="FollowersComponent">
    <va-list>
      <va-list-item v-for="(item, index) in followers" :key="index">
        <va-list-item-section>
          <va-list-item-label>
            {{ item.whomId }}
          </va-list-item-label>
        </va-list-item-section>
        <va-list-item-section icon>
          <va-icon
            class="icon"
            name="remove_red_eye"
            color="red"
            @click="handleItemClick(item)"
          />
        </va-list-item-section>
      </va-list-item>
      <div></div>
      <div></div>
      <div></div>
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
#FollowersComponent {

    .icon:hover {
        cursor: pointer;
    }

}
</style>
