import { readonly, reactive } from "vue";
import followersApi from "@/api/followers/follower.js";

const state = reactive({
  followers: [],
});

const mutations = {
  setFollowers: (followers) => {
    state.followers = followers;
  },

  removeFollower: (userId) => {
    state.followers = [
      ...state.followers.filter((followerItem) => followerItem.WhoId != userId),
    ];
  },
};

const actions = {
  getFollowers: async (userId) => {
    try {
      const result = await followersApi.followedUsers(userId);
      console.log("followedUsers", result)
      mutations.setFollowers(result);
    } catch (e) {
      console.log(e);
    }
  },

  unfollowUser: async (userId) => {
    try {
      await followersApi.unfollowUser(userId);
      mutations.removeFollower(userId);
    } catch (e) {
      console.log(e);
    }
  },
};

export default {
  state: readonly(state),
  actions: readonly(actions),
};
