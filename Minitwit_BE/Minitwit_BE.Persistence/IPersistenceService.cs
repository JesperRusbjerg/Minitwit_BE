using Minitwit_BE.Domain;

namespace Minitwit_BE.Persistence
{
    public interface IPersistenceService
    {
        Task<IEnumerable<Follower>> GetFollowers(Func<Follower, bool> func);
        Task AddFollower(Follower follower);
        Task DeleteFollower(Follower follower);

        Task<IEnumerable<Message>> GetMessages(Func<Message, bool> func);
        Task AddMessage(Message message);
        Task DeleteMessage(Message message);
        Task UpdateMessage(int id, Message message);

        Task<IEnumerable<User>> GetUsers(Func<User, bool> func);
        Task AddUser(User user);
        Task DeleteUser(Message user);

        Task UpdateLatest(int latest);
        Task<int> GetLatest();
    }
}
