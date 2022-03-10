using Minitwit_BE.Domain;
using Minitwit_BE.Domain.Exceptions;
using Minitwit_BE.DomainService.Interfaces;
using Minitwit_BE.Persistence;

namespace Minitwit_BE.DomainService
{
    public class MessageDomainService : IMessageDomainService
    {
        private readonly IPersistenceService _persistenceService;

        public MessageDomainService(IPersistenceService persistenceService)
        {
            _persistenceService = persistenceService;
        }

        public async Task AddTwit(Message msg)
        {
            await _persistenceService.AddMessage(msg);
        }

        public async Task AddTwit(Message msg, string username)
        {
            var user = (await _persistenceService.GetUsers(u => u.UserName.Equals(username))).FirstOrDefault();

            if (user == null)
                throw new ArgumentException("No user of that username exists");

            msg.AuthorId = user.UserId;

            await _persistenceService.AddMessage(msg);
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
            
            var personalTwits = (await _persistenceService.GetMessages(msg => msg.AuthorId.Equals(id) && msg.Flagged != true)).Take(number);

            return personalTwits;
        }

        public async Task<IEnumerable<Message>> GetPersonalTwits(string username)
        {
            var user = (await _persistenceService.GetUsers(u => u.UserName.Equals(username))).FirstOrDefault();

            if (user == null)
                throw new ArgumentException("No user of that username exists");
            
            return await GetPersonalTwits(user.UserId);
        }

        public async Task<IEnumerable<Message>> GetPersonalTwits(string username, int? numberOfRows)
        {
            var user = (await _persistenceService.GetUsers(u => u.UserName.Equals(username))).FirstOrDefault();

            if (user == null)
                throw new ArgumentException("No user of that username exists");
            
            return await GetPersonalTwits(user.UserId, numberOfRows);
        }

        public async Task MarkMessage(int msgId, bool flag)
        {
            var flaggedMessage = (await _persistenceService.GetMessages(msg => msg.MessageId.Equals(msgId))).SingleOrDefault();

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
