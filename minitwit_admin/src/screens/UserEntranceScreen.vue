<template>
    <div class="user-entrance-page">
        <user-form/>
        <user-form :ifRegistrationForm="true"/>
    </div>
</template>
<script>
import UserForm from '@/components/UserForm.vue';
import { computed, inject } from "vue";

export default {
    name: "UserEntranceScreen",
    props: {},
    components: {
        UserForm
    },
    setup() {
        const store = inject("store");
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
    padding-bottom: 20%;

    [class$="-form"] {
    }
}
</style>