using Microsoft.EntityFrameworkCore;
using Minitwit_BE.Domain;

namespace Minitwit_BE.Persistence
{
    public class TwitContext: DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Follower> Followers { get; set; }
        public DbSet<Message> Messages { get; set; }

        public string _dbPath { get; }

        public TwitContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            _dbPath = Path.Join(path, "twit.db");

            // for me it's C:\Users\<USER>\AppData\Local
        }

        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={_dbPath}");
    }
}