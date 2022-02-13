using Minitwit_BE.Domain;

namespace Minitwit_BE.DomainService.Interfaces
{
    public interface IMessageDomainService
    {
        Task AddTwit(Message msg);
        Task<IEnumerable<Message>> GetTwits();
        Task<IEnumerable<Message>> GetPersonalTwits(int id);
        Task MarkMessage(int msgId, bool flag);
    }
}
