import { reactive, computed, readonly } from "vue";
import followersApi from "@/api/followers/follower.js";
import { getLoggedInUser } from "@/compositionStore/users/usersModule.js";

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

  addFollower: (followObj) => {
    state.followers.push(followObj)
  }
};

const actions = {
  fetchFollowers: async (userId) => {
    try {
      const result = await followersApi.followedUsers(userId);
      mutations.setFollowers(result.length === 0 ? [] : result);
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

  followUser: async (userId) => {
    const isAlreadyFollowed = state.followers.some(entity => entity.whomId === userId)
    if (isAlreadyFollowed) return;
    if (!!getLoggedInUser().value) {
      try {
        const userData = {
          "WhoId": getLoggedInUser().value,
          "WhomId": userId
        }
        const res = await followersApi.followUser(userData);
        mutations.addFollower(res)
      } catch (e) {
        console.error(e);
      }
    }
  },
};

const getFollowers = () => computed(() => state.followers)
const fetchFollowers = (userId) => actions.fetchFollowers(userId)
const unfollowUser = (userId) => actions.unfollowUser(userId)
const followUser = (userId) => actions.followUser(userId)

export {
  getFollowers,
  fetchFollowers,
  unfollowUser,
  followUser
}

export default {
  state: readonly(state),
  mutations: readonly(mutations),
  actions: readonly(actions),
  getFollowers,
  fetchFollowers,
  unfollowUser,
  followUser
};
