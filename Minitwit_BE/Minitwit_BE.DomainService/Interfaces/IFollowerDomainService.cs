using Minitwit_BE.Domain;

namespace Minitwit_BE.DomainService.Interfaces
{
    public interface IFollowerDomainService
    {
        Task<IEnumerable<Follower>> GetFollowedUsers(int id);
        Task Follow(Follower follower);
        Task UnFollow(int id);
    }
}
