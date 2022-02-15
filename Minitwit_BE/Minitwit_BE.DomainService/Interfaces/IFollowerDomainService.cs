using Minitwit_BE.Domain;

namespace Minitwit_BE.DomainService.Interfaces
{
    public interface IFollowerDomainService
    {
        Task<IEnumerable<Follower>> GetFollowedUsers(int id);
        Task<IEnumerable<Follower>> GetFollowedUsers(string username);
        Task Follow(Follower follower);
        Task Follow(string userNameWho, string userNameWhom);
        Task UnFollow(int id);
        Task UnFollow(string userNameWho, string userNameWhom);
    }
}
