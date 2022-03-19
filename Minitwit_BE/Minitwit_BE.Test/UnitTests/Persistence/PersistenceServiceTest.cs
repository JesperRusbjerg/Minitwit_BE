using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Minitwit_BE.Domain;
using Minitwit_BE.Persistence;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minitwit_BE.Test
{
    public class PersistenceServiceTest
    {
        private IEnumerable<Follower> _followers;
        private IEnumerable<User> _users;
        private IEnumerable<Message> _messages;

        public PersistenceServiceTest()
        {
            _followers = new List<Follower>
            {
                new Follower
                {
                    Id = 1,
                    WhoId = 1,
                    WhomId = 2
                },
                new Follower
                {
                    Id = 2,
                    WhoId = 2,
                    WhomId = 1
                }
            };
            _users = new List<User>
            {
                new User
                {
                    Email = "email",
                    PwHash = "hash",
                    UserName = "name"
                },
                new User
                {
                    Email = "email1",
                    PwHash = "hash1",
                    UserName = "name1"
                }
            };
            _messages = new List<Message>
            {
                new Message
                {
                    AuthorId = 1,
                    Flagged = false,
                    PublishDate = DateTime.Now,
                    Text = "text1"
                },
                new Message
                {
                    AuthorId = 2,
                    Flagged = false,
                    PublishDate = DateTime.Now,
                    Text = "text1"
                },
                new Message
                {
                    AuthorId = 3,
                    Flagged = false,
                    PublishDate = DateTime.Now,
                    Text = "text1"
                },
            };
        }

        private async Task<TwitContext> GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<TwitContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var databaseContext = new TwitContext(options);
            databaseContext.Database.EnsureCreated();

            foreach (var f in _followers)
            {
                databaseContext.Followers.Add(new Follower()
                {
                    WhoId = f.WhoId,
                    WhomId = f.WhomId
                });
            }

            foreach (var u in _users)
            {
                databaseContext.Users.Add(new User()
                {
                    Email = u.Email,
                    PwHash = u.PwHash,
                    UserName = u.UserName
                });
            }

            foreach (var m in _messages)
            {
                databaseContext.Messages.Add(new Message()
                {
                    AuthorId = m.AuthorId,
                    Flagged = m.Flagged,
                    PublishDate = m.PublishDate,
                    Text = m.Text
                });
            }

            await databaseContext.SaveChangesAsync();

            return databaseContext;
        }

        [Test]
        public async Task FollowersTest()
        {
            //Arrange
            PersistenceService service = new PersistenceService(await GetDatabaseContext());
            Func<Follower, bool> func = func => true;
            var newFollower = new Follower
            {
                WhoId = 99,
                WhomId = 99
            };

            //Act
            await service.AddFollower(newFollower);
            await service.DeleteFollower(newFollower);
            var result = await service.GetFollowers(func);

            //Assert
            result.Should().BeEquivalentTo(_followers);
        }

        [Test]
        public async Task MessageTest()
        {
            //Arrange
            var expectedMessages = _messages;
            expectedMessages.FirstOrDefault().Text = "Kupa";

            PersistenceService service = new PersistenceService(await GetDatabaseContext());
            Func<Message, bool> func = func => true;

            var newMessage = new Message()
            {
                AuthorId = 55,
                Flagged = false,
                PublishDate = DateTime.Now,
                Text = "Text"
            };

            //Act
            await service.AddMessage(newMessage);
            await service.DeleteMessage(newMessage);

            var toBeUpdated = (await service.GetMessages(func)).FirstOrDefault();
            toBeUpdated.Text = "Kupa";
            await service.UpdateMessage(toBeUpdated.MessageId, toBeUpdated);

            var result = await service.GetMessages(func);

            //Assert
            result.Should().BeEquivalentTo(expectedMessages, opt => opt.Excluding(m => m.MessageId));
        }

        [Test]
        public async Task UsersTest()
        {
            //Arrange
            PersistenceService service = new PersistenceService(await GetDatabaseContext());
            Func<User, bool> func = func => true;
            var newUser = new User
            {
                Email = "e",
                PwHash = "p",
                UserName = "en"
            };

            //Act
            await service.AddUser(newUser);
            await service.DeleteUser(newUser);
            var result = await service.GetUsers(func);

            //Assert
            result.Should().BeEquivalentTo(_users, opt => opt.Excluding(u => u.UserId));
        }
    }
}