import { apiRequest } from '@/api/api.js';

const registerUser = (registrationData) => {
    return apiRequest("POST", "api/user/register", registrationData)
}

const loginUser = (loginData) => {
    return apiRequest("POST", "api/user/login", loginData)
}

export default {
    registerUser,
    loginUser
}