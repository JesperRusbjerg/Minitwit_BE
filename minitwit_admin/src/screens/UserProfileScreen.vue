<template>
    <div class="user-profile-page">
        <div class="followers">
        </div>
        <add-twit-component></add-twit-component>
    </div>
</template>
<script>
import AddTwitComponent from '@/components/AddTwitComponent.vue';
import { computed, inject } from "vue";

export default {
    name: "UserProfileScreen",
    props: {},
    components: {
        AddTwitComponent
    },
    setup() {
        return {};
    }
}
</script>
<style lang="scss">
.user-entrance-page {
    display: flex;
    flex-direction: row;
    justify-content: space-around;
    align-items: center;
    height: 100%;
    width: 100%;

    [class$="-form"] {
    }
}
</style>