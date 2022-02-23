<template>
    <div class="user-profile-page">
        <div class="followers-wrapper">
            <p> People you follow </p>
            <followers-component class="followed-users-list" :items="followers" @onClick="handleOnUnfollowClick" />
        </div>
        <div class="create-twit-wrapper">
            <add-twit-component></add-twit-component>
        </div>
        <div class="priv-twits-wrapper">
            <p> Your published twits </p>
            <twit-list-component :items="twitList" @onClick="handleOnTwitClick"/>
        </div>
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
            unfollowUser(item.id)
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
@import '../_variables.scss';

.user-profile-page {
    display: grid;
    grid-template-rows: [first] 35%[second] 65% [endline] auto;
    grid-template-columns: [col-first] 60% [col-sec] 40% [col-endline] auto;
    align-items: center;
    height: 100%;
    width: 100%;
    overflow: hidden;
    padding-top: 2rem;
    min-height: 750px;
    box-sizing: border-box;

    p { 
        font-weight: bold;
    }

    [class$="-wrapper"] {
        width: 100%;
        height: 100%;
        justify-self: center;
        align-self: start;

        > * {
            margin: 0 auto auto !important;
        }
    }

    .followers-wrapper {
        overflow: scroll;
        grid-area: 1 / col-sec / endline / col-endline;
    }

    .create-twit-wrapper {
        grid-area: 1 / 1 / second / col-sec;
    }

    .priv-twits-wrapper {
        grid-area: second / 1 / endline / col-sec;

        > div {
            border-top: 1px solid $border-background;
            width: 90% !important;
            height: 100%!important;
            overflow: scroll;
        }

        

        .va-list {
            width: 90%;
        }

        .va-list-label {
            display: none;
        }
    }
}
</style>