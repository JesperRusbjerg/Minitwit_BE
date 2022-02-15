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

        public async Task<IEnumerable<Message>> GetTwits()
        {
            return await _persistenceService.GetMessages(e => true);
        }

        public async Task<IEnumerable<Message>> GetPersonalTwits(int id)
        {
            return await _persistenceService.GetMessages(msg => msg.AuthorId.Equals(id));
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
