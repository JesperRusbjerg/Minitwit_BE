using Minitwit_BE.Domain;
using Minitwit_BE.Domain.Exceptions;
using Minitwit_BE.DomainService.Interfaces;
using Minitwit_BE.Persistence;
using Prometheus;

namespace Minitwit_BE.DomainService
{
    public class MessageDomainService : IMessageDomainService
    {
        private readonly Histogram _tweetOperationDuration;


        private readonly IPersistenceService _persistenceService;

        public MessageDomainService(IPersistenceService persistenceService)
        {
            _persistenceService = persistenceService;
            _tweetOperationDuration = Metrics.CreateHistogram("TweetOperationDuration", "Measures the time it takes to create a tweet");
        }

        public async Task AddTwit(Message msg)
        {
            using (_tweetOperationDuration.NewTimer())
            {
                await _persistenceService.AddMessage(msg);
            }
        }

        public async Task AddTwit(Message msg, string username)
        {
            using (_tweetOperationDuration.NewTimer())
            {
                var user = (await _persistenceService.GetUsers(u => u.UserName.Equals(username))).FirstOrDefault();

                if (user == null)
                    throw new UserNotFoundException("No user of that username exists");

                msg.AuthorId = user.UserId;

                await _persistenceService.AddMessage(msg);
            }
        }

        public async Task<IEnumerable<Message>> GetTwits()
        {
            return await _persistenceService.GetMessages(e => e.Flagged != true);
        }

        public async Task<IEnumerable<Message>> GetTwits(int? numberOfRows)
        {
            var number = numberOfRows ?? 100;
            return (await _persistenceService.GetMessages(e => e.Flagged != true)).Take(number);
        }

        public async Task<IEnumerable<Message>> GetPersonalTwits(int id)
        {
            return await _persistenceService.GetMessages(msg => msg.AuthorId.Equals(id) && msg.Flagged != true);
        }

        public async Task<IEnumerable<Message>> GetPersonalTwits(int id, int? numberOfRows)
        {

            var number = numberOfRows ?? 100;

            var personalTwits = (await _persistenceService.GetMessages(msg => msg.AuthorId.Equals(id) && msg.Flagged != true)).Reverse().Take(number);

            return personalTwits;
        }

        public async Task<IEnumerable<Message>> GetPersonalTwits(string username)
        {
            var user = (await _persistenceService.GetUsers(u => u.UserName.Equals(username))).FirstOrDefault();

            if (user == null)
                throw new UserNotFoundException("No user of that username exists");

            return await GetPersonalTwits(user.UserId);
        }

        public async Task<IEnumerable<Message>> GetPersonalTwits(string username, int? numberOfRows)
        {
            var user = (await _persistenceService.GetUsers(u => u.UserName.Equals(username))).FirstOrDefault();

            if (user == null)
                throw new UserNotFoundException("No user of that username exists");

            return await GetPersonalTwits(user.UserId, numberOfRows);
        }

        public async Task MarkMessage(int msgId, bool flag)
        {
            var flaggedMessage = (await _persistenceService.GetMessages(msg => msg.MessageId.Equals(msgId))).FirstOrDefault();

            if (flaggedMessage != null)
            {
                flaggedMessage.Flagged = flag;
                await _persistenceService.UpdateMessage(msgId, flaggedMessage);
            }
            else
            {
                throw new MessageNotFoundException("Twit with given id does not exist!");
            }
        }
    }
}
