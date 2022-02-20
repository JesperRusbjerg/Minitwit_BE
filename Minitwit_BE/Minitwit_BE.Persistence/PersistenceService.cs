using Minitwit_BE.Domain;

namespace Minitwit_BE.Persistence
{
    // It's not recommended to run tasks async on the same DbContext, hence I'm wrapping them in a task. It's not ideal, but allowes
    // us to still use async framework on other calls beforehand.

    public class PersistenceService : IPersistenceService
    {
        private readonly TwitContext _twitContext;

        public PersistenceService(TwitContext twitContext)
        {
            _twitContext = twitContext;
        }

        #region Followers
        public async Task<IEnumerable<Follower>> GetFollowers(Func<Follower, bool> func)
        {
            return await Task.FromResult(_twitContext.Followers.Where(func).AsEnumerable());
        }

        public async Task AddFollower(Follower follower)
        {
            await Task.Run(() =>
            {
                _twitContext.Add(follower);
                _twitContext.SaveChanges();
            });
        }

        public async Task DeleteFollower(Follower follower)
        {
            await Task.Run(() =>
            {
                _twitContext.Remove(follower);
                _twitContext.SaveChanges();
            });
        }
        #endregion

        #region Message
        public async Task<IEnumerable<Message>> GetMessages(Func<Message, bool> func)
        {
            return await Task.FromResult(_twitContext.Messages.Where(func).AsEnumerable());
        }

        public async Task AddMessage(Message message)
        {
            await Task.Run(() =>
            {
                _twitContext.Add(message);
                _twitContext.SaveChanges();
            });
        }

        public async Task DeleteMessage(Message message)
        {
            await Task.Run(() => 
            {
                _twitContext.Remove(message);
                _twitContext.SaveChanges();
            });
        }

        public async Task UpdateMessage(int id, Message message)
        {
            await Task.Run(() =>
            {
                var flaggedMsg = _twitContext.Messages.SingleOrDefault(msg => msg.MessageId.Equals(id));

                if (flaggedMsg != null)
                {
                    flaggedMsg.Text = message.Text;
                    flaggedMsg.PublishDate = message.PublishDate;
                    flaggedMsg.Flagged = message.Flagged;
                    _twitContext.SaveChanges();
                }
            });
        }
        #endregion

        #region Users
        public async Task<IEnumerable<User>> GetUsers(Func<User, bool> func)
        {
            return await Task.FromResult(_twitContext.Users.Where(func).AsEnumerable());
        }

        public async Task AddUser(User user)
        {
            await Task.Run(() =>
           {
            _twitContext.Add(user);
            _twitContext.SaveChanges();
           });
        }

        public async Task DeleteUser(Message user)
        {
            await Task.Run(() =>
            {
                _twitContext.Remove(user);
                _twitContext.SaveChanges();
            });
        }
        #endregion

        #region Simulator
        private static int _latest = 0; // TODO: This should be stored in DB
        public async Task UpdateLatest(int latest)
        {
            await Task.Run(() => _latest = latest);
        }

        public Task<int> GetLatest()
        {
            return Task.Run(() => _latest);
        }
        #endregion
    }
}
