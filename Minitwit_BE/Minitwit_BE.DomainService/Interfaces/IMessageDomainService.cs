using Minitwit_BE.Domain;

namespace Minitwit_BE.DomainService.Interfaces
{
    public interface IMessageDomainService
    {
        Task AddTwit(Message msg);
        Task AddTwit(Message msg, string username);
        Task<IEnumerable<Message>> GetTwits();
        Task<IEnumerable<Message>> GetTwits(int? numberOfRows);
        Task<IEnumerable<Message>> GetPersonalTwits(int id);
        Task<IEnumerable<Message>> GetPersonalTwits(string username);
        Task MarkMessage(int msgId, bool flag);
    }
}
