using Minitwit_BE.Domain;

namespace Minitwit_BE.DomainService.Interfaces
{
    public interface IFollowerDomainService
    {
        Task<IEnumerable<Follower>> GetFollowedUsers(int id);
        Task<IEnumerable<Follower>> GetFollowedUsers(int id, int? numberOfRows);
        Task<IEnumerable<Follower>> GetFollowedUsers(string username);
        Task<IEnumerable<Follower>> GetFollowedUsers(string username, int? numberOfRows);
        Task Follow(Follower follower);
        Task<int> Follow(string userNameWho, string userNameWhom);
        Task UnFollow(int id);
        Task<int> UnFollow(string userNameWho, string userNameWhom);
    }
}
