import { apiRequest } from '@/api/api.js';

const followedUsers = (userId) => {
    return apiRequest("GET", `api/followers/list/${userId}`)
}

const followUser = (followData) => {
    return apiRequest("GET", "api/followers/follow", followData)
}

const followUser = (followEntryId) => {
    return apiRequest("GET", `api/followers/follow/${followEntryId}`)
}

export default {
    followUser,
    followUser,
    unfollowUser
}
