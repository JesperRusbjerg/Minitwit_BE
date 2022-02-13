<template>
    <div class="user-profile-page">
        <div class="followers">
            <followers-component :items="followers" @onClick="handleOnUnfollowClick" />
        </div>
        <add-twit-component></add-twit-component>
    </div>
</template>
<script>
import AddTwitComponent from '@/components/AddTwitComponent.vue';
import FollowersComponent from '@/components/FollowersComponent.vue';
import { computed, inject } from "vue";

export default {
    name: "UserProfileScreen",
    props: {},
    components: {
        AddTwitComponent,
        FollowersComponent,
    },
    setup() {
        const store = inject("store")
        const loggedInUserId = computed(() => store.users.state.loggedUser)
        const followers = computed(() => store.followers.state.followers)

        const unfollowUser = (userId) => {
            store.followers.actions.unfollowUser(userId)
        }
        const getFollowers = () => {
            store.followers.actions.getFollowers(loggedInUserId.value)
        }

        const handleOnUnfollowClick = (item) => {
            unfollowUser(item.whomId)
        }

        getFollowers()
        return {
            followers,
            handleOnUnfollowClick
        };
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