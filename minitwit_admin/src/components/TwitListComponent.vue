<template>
  <div id="TwitListComponent" :style="{ height: height, width: width }">
    <va-list>
      <va-list-label class="label" :color="labelColor">
        {{ label }}
      </va-list-label>

      <va-list-item
        v-for="(item, index) in items"
        :key="index"
        :style="{
          backgroundColor: itemBackgroundColor,
          padding: itemPadding,
          margin: itemMargin,
        }"
      >
        <va-list-item-section class="content">
          <div class="header">
            <va-list-item-label>
              Author ID: {{ item.authorId }}
            </va-list-item-label>
            <va-list-item-label>
              Message ID: {{ item.messageId }}
            </va-list-item-label>
          </div>
          <va-list-item-label :color="textColor">
            {{ item.text }}
          </va-list-item-label>
        </va-list-item-section>

        <va-list-item-section icon>
          <va-icon
            class="icon"
            :name="item.flagged ? 'house' : 'house'"
            :color="item.flagged ? 'green' : 'red'"
            @click="handleItemClick(item)"
          />
        </va-list-item-section>
      </va-list-item>
    </va-list>
  </div>
</template>

<script>
export default {
  name: "TwitListComponent",
  props: {
    items: {
      type: Array,
      required: true,
      default: [],
    },
    width: {
      type: String,
      required: false,
      default: "400px",
    },
    height: {
      type: String,
      required: false,
      default: "500px",
    },
    label: {
      type: String,
      required: false,
      default: "Twits",
    },
    labelColor: {
      type: String,
      required: false,
      default: "#FFF",
    },
    textColor: {
      type: String,
      required: false,
      default: "#FFF",
    },
    itemBackgroundColor: {
      type: String,
      required: false,
      default: "#AAAAAA",
    },
    itemPadding: {
      type: String,
      required: false,
      default: "12px",
    },
    itemMargin: {
      type: String,
      required: false,
      default: "12px",
    },
  },
  components: {},
  emits: ["onClick"],
  setup(props, context) {
    const handleItemClick = (item) => {
      context.emit("onClick", item);
    };
    return {
      handleItemClick,
    };
  },
};
</script>

<style lang="scss" scoped>
#TwitListComponent {
  .icon {
    &:hover {
      cursor: pointer;
    }
  }
}
</style>
