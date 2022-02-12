using Microsoft.Extensions.Logging;
using Minitwit_BE.Domain;
using Minitwit_BE.Persistence;

namespace Minitwit_BE.DomainService
{
    public class FollowerDomainService
    {
        private readonly PersistenceService _persistence;
        private readonly ILogger<FollowerDomainService> _logger;

        public FollowerDomainService(PersistenceService persistence, ILogger<FollowerDomainService> logger)
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

            var deletedFollow = _persistence.GetFollowers(queryExpresion).Result.FirstOrDefault();

            if (deletedFollow != null)
            {
                _persistence.DeleteFollower(deletedFollow);
            }
            else
            {
                throw new InvalidOperationException("That user is not currently followed");
            }
        }
    }
}