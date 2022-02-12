import { apiRequest } from "@/api/api.js";

const fetchTwits = () => {
    return apiRequest("GET", "api/twit/public-twits")
};

const flagTwit = (flaggingInput) => {
    return apiRequest("PUT", "api/twit/mark-message", flaggingInput)
}

export default {
    fetchTwits,
    flagTwit
}
