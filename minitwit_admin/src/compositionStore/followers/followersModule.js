import { reactive, computed } from "vue";
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
      ...state.followers.filter((followerItem) => followerItem.whoId != userId),
    ];
  },
};

const actions = {
  fetchFollowers: async (userId) => {
    try {
      const result = await followersApi.followedUsers(userId);
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

const getFollowers = () => computed(() => state.followers)
const fetchFollowers = (userId) => actions.fetchFollowers(userId)
const unfollowUser = (userId) => actions.unfollowUser(userId)

export {
  getFollowers,
  fetchFollowers,
  unfollowUser
}

export default {
  getFollowers,
  fetchFollowers,
  unfollowUser
};
