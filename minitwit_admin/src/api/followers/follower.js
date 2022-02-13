import { apiRequest } from '@/api/api.js';

const followedUsers = (userId) => {
    return apiRequest("GET", `api/followers/list/${userId}`)
}

const followUser = (followData) => {
    return apiRequest("POST", "api/followers/follow", followData)
}

const unfollowUser = (followEntryId) => {
    return apiRequest("DELETE", `api/followers/unfollow/${followEntryId}`)
}

export default {
    followedUsers,
    followUser,
    unfollowUser
}
