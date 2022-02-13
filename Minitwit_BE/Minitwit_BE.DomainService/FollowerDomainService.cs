using Microsoft.Extensions.Logging;
using Minitwit_BE.Domain;
using Minitwit_BE.Domain.Exceptions;
using Minitwit_BE.DomainService.Interfaces;
using Minitwit_BE.Persistence;

namespace Minitwit_BE.DomainService
{
    public class FollowerDomainService : IFollowerDomainService
    {
        private readonly IPersistenceService _persistence;
        private readonly ILogger<FollowerDomainService> _logger;

        public FollowerDomainService(IPersistenceService persistence, ILogger<FollowerDomainService> logger)
        {
            _persistence = persistence;
            _logger = logger;
        }

        public async Task<IEnumerable<Follower>> GetFollowedUsers(int id)
        {
            Func<Follower, bool> queryExpresion = entry => entry.WhoId.Equals(id);

            var followersList = _persistence.GetFollowers(queryExpresion);

            return await followersList;
        }

        public async Task Follow(Follower follower)
        {
            await _persistence.AddFollower(follower);
        }

        public async Task UnFollow(int id)
        {
            Func<Follower, bool> queryExpresion = entry => entry.WhoId.Equals(id);

            var getFollowsTask = await _persistence.GetFollowers(queryExpresion);

            var deletedFollow = getFollowsTask.FirstOrDefault();

            if (deletedFollow != null)
            {
                await _persistence.DeleteFollower(deletedFollow);
            }
            else
            {
                throw new UserUnfollowException("Cannot unfollow this user, as it is not currently followed!");
            }
        }
    }
}