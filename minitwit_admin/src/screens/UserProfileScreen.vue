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
import { useFollowers, useTwits, useUsers } from "@/compositionStore/index"

export default {
    name: "UserProfileScreen",
    props: {},
    components: {
        AddTwitComponent,
        FollowersComponent,
        TwitListComponent
    },
    setup() {
        const { getFollowers, fetchFollowers, unfollowUser } = useFollowers()
        const { getPrivateTwitList, fetchPrivateTwitList, flagTwit } = useTwits()
        const { getLoggedInUser } = useUsers()
        const loggedInUserId = getLoggedInUser()

        const handleOnUnfollowClick = (item) => {
            unfollowUser(item.whoId)
        }

        const handleOnTwitClick = (twit) => {
            flagTwit(twit.messageId, twit.flagged)
        }

        fetchPrivateTwitList(loggedInUserId.value)
        fetchFollowers(loggedInUserId.value)
        return {
            followers: getFollowers(),
            handleOnUnfollowClick,
            handleOnTwitClick,
            twitList: getPrivateTwitList()
        };
    }
}
</script>
<style lang="scss">
.user-profile-page {
    display: flex;
    flex-direction: column;
    justify-content: center;
    align-items: center;
    height: 100%;
    width: 100%;

    [class$="-form"] {
        
    }
}
</style>