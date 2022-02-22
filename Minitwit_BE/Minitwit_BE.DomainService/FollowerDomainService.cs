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

        public async Task<IEnumerable<Follower>> GetFollowedUsers(string username)
        {
            var user = (await _persistence.GetUsers(u => u.UserName.Equals(username))).SingleOrDefault();

            if (user == null)
                throw new ArgumentException("User does not exist!");
            
            return await GetFollowedUsers(user.UserId);
        }

        public async Task<IEnumerable<Follower>> GetFollowedUsers(string username, int? numberOfRows)
        {
            var user = (await _persistence.GetUsers(u => u.UserName.Equals(username))).SingleOrDefault();

            if (user == null)
                throw new ArgumentException("User does not exist!");
            
            return await GetFollowedUsers(user.UserId, numberOfRows);
        }

        public async Task<IEnumerable<Follower>> GetFollowedUsers(int id)
        {
            var followersList = _persistence.GetFollowers(entry => entry.WhoId.Equals(id));

            return await followersList;
        }

        public async Task<IEnumerable<Follower>> GetFollowedUsers(int id, int? numberOfRows)
        {
            var number = numberOfRows ?? 100;
            var followersList = (await _persistence.GetFollowers(entry => entry.WhoId.Equals(id))).Take(number);

            return followersList;
        }

        public async Task Follow(Follower follower)
        {
            var userWhom = (await _persistence.GetUsers(u => u.UserId.Equals(follower.WhomId))).SingleOrDefault();

            if (userWhom == null)
            {
                throw new ArgumentException("Users might not exist");
            } else
            {
                var followings = (await GetFollowedUsers(userWhom.UserId)).ToList();
                var isAlreadyFollowing = followings.Any(item => item.WhomId.Equals(userWhom.UserId));

                if (isAlreadyFollowing)
                {
                    throw new ArgumentException("Users might not exist");
                }
                else
                {
                    await _persistence.AddFollower(follower);
                }
            }
        }

        public async Task<int> Follow(string userNameWho, string userNameWhom)
        {
            var userWhom = (await _persistence.GetUsers(u => u.UserName.Equals(userNameWhom))).SingleOrDefault();
            if (userWhom == null)
            {
                return 1;
            } else 
            {
                var followings = (await GetFollowedUsers(userNameWho)).ToList();
                var isAlreadyFollowing = followings.Any(item => item.WhomId.Equals(userWhom.UserId));

                if (isAlreadyFollowing)
                {
                    return 1;
                } else
                {
                    var userWho = (await _persistence.GetUsers(u => u.UserName.Equals(userNameWho))).SingleOrDefault();

                    if (userWho == null)
                        throw new ArgumentException("Users might not exist");

                    await _persistence.AddFollower(new Follower
                    {
                        WhoId = userWho.UserId,
                        WhomId = userWhom.UserId,
                    });

                    return 0;
                }
            }
        }

        public async Task UnFollow(int id)
        {
            var deletedFollow = (await _persistence.GetFollowers(entry => entry.Id.Equals(id))).FirstOrDefault();

            if (deletedFollow != null)
            {
                await _persistence.DeleteFollower(deletedFollow);
            }
            else
            {
                throw new UserUnfollowException("Cannot unfollow this user, as it is not currently followed!");
            }
        }

        public async Task<int> UnFollow(string userNameWho, string userNameWhom)
        {
            var userWhoTask = _persistence.GetUsers(u => u.UserName.Equals(userNameWho));
            var userWhomTask = _persistence.GetUsers(u => u.UserName.Equals(userNameWhom));
            var taskList = new List<Task> { userWhoTask, userWhomTask };

            await Task.WhenAll(taskList);

            if (userWhoTask.Result.SingleOrDefault() == null || userWhomTask.Result.SingleOrDefault() == null)
            {
                return 1;
            }
            else
            {
                var follower = new Follower
                {
                    WhoId = userWhoTask.Result.SingleOrDefault().UserId,
                    WhomId = userWhomTask.Result.SingleOrDefault().UserId
                };

                var deletedFollow = (await _persistence.GetFollowers(
                    entry => entry.WhoId.Equals(follower.WhoId) && entry.WhomId.Equals(follower.WhomId))).SingleOrDefault();

                if (deletedFollow != null)
                {
                    await _persistence.DeleteFollower(deletedFollow);
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
        }
    }
}