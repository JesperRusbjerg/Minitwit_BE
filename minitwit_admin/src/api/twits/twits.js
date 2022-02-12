import { apiRequest } from "@/api/api.js";

const fetchTwits = () => {
    return apiRequest("GET", "api/twits/public-twits")
};

const flagTwit = (flaggingInput) => {
    return apiRequest("PUT", "api/twit/mark-message", flaggingInput)
}

export default {
    fetchTwits,
    flagTwit
}

const mockTwitList = [
  {
    messageId: 1,
    authorId: 1,
    text: "jjjjjjjjjjjjjjj",
    publishDate: "2022-02-09T19:28:07.8611711",
    flagged: false,
  },
  {
    messageId: 2,
    authorId: 1,
    text: "jjjjjjjjjjjjjjj",
    publishDate: "2022-02-09T19:28:08.8306713",
    flagged: false,
  },
  {
    messageId: 3,
    authorId: 1,
    text: "jjjjjjjjjjjjjjj",
    publishDate: "2022-02-09T19:28:09.3920778",
    flagged: false,
  },
  {
    messageId: 4,
    authorId: 1,
    text: "jjjjjjjjjjjjjjj",
    publishDate: "2022-02-09T19:28:09.9205645",
    flagged: false,
  },
  {
    messageId: 5,
    authorId: 1,
    text: "jjjjjjjjjjjjjjj",
    publishDate: "2022-02-09T19:28:31.240109",
    flagged: false,
  },
  {
    messageId: 6,
    authorId: 1,
    text: "jjjjjjjjjjjjjjj",
    publishDate: "2022-02-09T19:42:18.2473394",
    flagged: false,
  },
  {
    messageId: 7,
    authorId: 1,
    text: "jjjjjjjjjjjjjjj",
    publishDate: "2022-02-09T19:44:23.0605588",
    flagged: false,
  },
  {
    messageId: 8,
    authorId: 1,
    text: "jjjjjjjjjjjjjjj",
    publishDate: "2022-02-09T19:44:24.6919686",
    flagged: false,
  },
];
