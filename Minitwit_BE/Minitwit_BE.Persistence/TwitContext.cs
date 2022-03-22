using Microsoft.EntityFrameworkCore;
using Minitwit_BE.Domain;

namespace Minitwit_BE.Persistence
{
    public class TwitContext: DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Follower> Followers { get; set; }
        public DbSet<Message> Messages { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public TwitContext(DbContextOptions<TwitContext> options): base(options) { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        protected override void OnModelCreating(ModelBuilder modelBuilder) { }
    }
}