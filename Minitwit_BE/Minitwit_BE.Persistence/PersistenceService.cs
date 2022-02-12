using Minitwit_BE.Domain;

namespace Minitwit_BE.Persistence
{
    public class PersistenceService
    {
        private readonly TwitContext _twitContext;

        public PersistenceService(TwitContext twitContext)
        {
            _twitContext = twitContext;
        }

        #region Followers
        public async Task<IEnumerable<Follower>> GetFollowers(Func<Follower, bool> func)
        {
            return _twitContext.Followers.Where(func).AsEnumerable().ToList();
        }

        public async Task AddFollower(Follower follower)
        {
            _twitContext.Add(follower);
            _twitContext.SaveChanges();
        }

        public async Task DeleteFollower(Follower follower)
        {
            _twitContext.Remove(follower);
            _twitContext.SaveChanges();
        }
        #endregion

        #region Message
        public async Task<IEnumerable<Message>> GetMessages(Func<Message, bool> func)
        {
            return _twitContext.Messages.Where(func).AsEnumerable();
        }

        public async Task AddMessage(Message message)
        {
            _twitContext.Add(message);
            _twitContext.SaveChanges();
        }

        public async Task DeleteMessage(Message message)
        {
            _twitContext.Remove(message);
            _twitContext.SaveChanges();
        }

        public async Task UpdateMessage(int id, Message message)
        {
            var flaggedMsg = _twitContext.Messages.SingleOrDefault(msg => msg.MessageId.Equals(id));

            if (flaggedMsg != null)
            {
                flaggedMsg.Text = message.Text;
                flaggedMsg.PublishDate = message.PublishDate;
                flaggedMsg.Flagged = message.Flagged;
                _twitContext.SaveChanges();
            }
        }
        #endregion

        #region Users
        public async Task<IEnumerable<User>> GetUsers(Func<User, bool> func)
        {
            return _twitContext.Users.Where(func).AsEnumerable();
        }

        public async Task AddUser(User user)
        {
            _twitContext.Add(user);
            _twitContext.SaveChanges();
        }

        public async Task DeleteUser(Message user)
        {
            _twitContext.Remove(user);
            _twitContext.SaveChanges();
        }
        #endregion
    }
}
