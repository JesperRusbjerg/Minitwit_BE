<template>
    <div class="user-profile-page">
        <div class="followers">
            <followers-component :items="followers" @onClick="handleOnUnfollowClick" />
        </div>
        <add-twit-component></add-twit-component>
        <twit-list-component :items="twitList" @onClick="handleOnTwitClick"/>
    </div>
</template>
<script>
import AddTwitComponent from '@/components/AddTwitComponent.vue';
import FollowersComponent from '@/components/FollowersComponent.vue';
import TwitListComponent from "@/components/TwitListComponent.vue";
import { computed, inject } from "vue";

export default {
    name: "UserProfileScreen",
    props: {},
    components: {
        AddTwitComponent,
        FollowersComponent,
        TwitListComponent
    },
    setup() {
        const store = inject("store")
        const loggedInUserId = computed(() => store.users.state.loggedUser)
        const followers = computed(() => store.followers.state.followers)
        const twitList = computed(() => store.twits.state.usersTwitList);

        const unfollowUser = (userId) => {
            store.followers.actions.unfollowUser(userId)
        }
        const getFollowers = () => {
            store.followers.actions.getFollowers(loggedInUserId.value)
        }

        const handleOnUnfollowClick = (item) => {
            unfollowUser(item.whomId)
        }

        const getTwitList = () => store.twits.actions.getUsersTwitList(loggedInUserId.value);

        const flagTwit = (messageId, flagged) => {
            store.twits.actions.toggleFlag(messageId, flagged)
        }

        const handleOnTwitClick = (twit) => {
            flagTwit(twit.messageId, twit.flagged)
        }
        getTwitList()
        getFollowers()
        return {
            followers,
            handleOnUnfollowClick,
            handleOnTwitClick,
            twitList
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