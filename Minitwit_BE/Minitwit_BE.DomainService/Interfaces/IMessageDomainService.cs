using Minitwit_BE.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        Task<IEnumerable<Message>> GetPersonalTwits(string username, int? numberOfRows);
        Task MarkMessage(int msgId, bool flag);
    }
}
