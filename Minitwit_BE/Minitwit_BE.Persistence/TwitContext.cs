using Microsoft.EntityFrameworkCore;
using Minitwit_BE.Domain;

namespace Minitwit_BE.Persistence
{
    public class TwitContext: DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Follower> Followers { get; set; }
        public DbSet<Message> Messages { get; set; }

        public TwitContext(DbContextOptions<TwitContext> options): base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder) { }
    }
}