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
            var follow = (await _persistence.GetFollowers(u => u.WhoId.Equals(username))).SingleOrDefault();

            if (follow == null)
                throw new ArgumentException("User does not exist!");
            
            return await GetFollowedUsers(follow.WhoId);
        }

        public async Task<IEnumerable<Follower>> GetFollowedUsers(int id)
        {
            var followersList = _persistence.GetFollowers(entry => entry.WhoId.Equals(id));

            return await followersList;
        }

        public async Task Follow(Follower follower)
        {
            await _persistence.AddFollower(follower);
        }

        public async Task Follow(string userNameWho, string userNameWhom)
        {
            var userWhoTask = _persistence.GetUsers(u => u.UserName.Equals(userNameWho));
            var userWhomTask = _persistence.GetUsers(u => u.UserName.Equals(userNameWhom));
            var taskList = new List<Task> { userWhoTask, userWhomTask };

            await Task.WhenAll(taskList);
            var userWho = userWhoTask.Result.SingleOrDefault();
            var userWhom = userWhomTask.Result.SingleOrDefault();

            if (userWho == null || userWhom == null)
                throw new ArgumentException("Users do not exist");

            await _persistence.AddFollower(new Follower
            {
                WhoId = userWho.UserId,
                WhomId = userWhom.UserId,
            });
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

        public async Task UnFollow(string userNameWho, string userNameWhom)
        {
            var userWhoTask = _persistence.GetUsers(u => u.UserName.Equals(userNameWho));
            var userWhomTask = _persistence.GetUsers(u => u.UserName.Equals(userNameWhom));
            var taskList = new List<Task> { userWhoTask, userWhomTask };

            await Task.WhenAll(taskList);

            if (userWhoTask.Result.SingleOrDefault() == null || userWhomTask.Result.SingleOrDefault() == null)
            {
                throw new ArgumentException("Users do not exist");
            }
            else
            {
                var follower = new Follower
                {
                    WhoId = userWhoTask.Result.SingleOrDefault().UserId,
                    WhomId = userWhomTask.Result.SingleOrDefault().UserId
                };

                await _persistence.DeleteFollower(follower);
            }
        }
    }
}