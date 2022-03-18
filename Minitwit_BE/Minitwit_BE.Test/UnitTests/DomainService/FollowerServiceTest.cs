using FluentAssertions;
using Minitwit_BE.Domain;
using Minitwit_BE.DomainService;
using Minitwit_BE.Persistence;
using Moq;
using Moq.AutoMock;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minitwit_BE.Test
{
    public class FollowerServiceTest
    {
        class GetFollowedUsersTest
        {
            [Test]
            public async Task GetFollowedUsers_When_ValidUsernameProvidedAndNumberOfRows_Then_ReturnsListOfFollowers()
            {
                //Arrange
                var mock = new AutoMocker();
                var username = "name";
                var id = 1;
                IEnumerable<User> persistenceUser = new List<User> { new User { UserId = id } };
                IEnumerable<Follower> expectedListOfFollowers = new List<Follower>
            {
                new Follower
                {
                    Id = 1,
                    WhoId = 2,
                    WhomId = id,
                }
            };

                var persistenceServiceMock = mock.GetMock<IPersistenceService>();
                persistenceServiceMock.Setup(p => p.GetUsers(It.IsAny<Func<User, bool>>()))
                    .Returns(Task.FromResult(persistenceUser)).Verifiable();
                persistenceServiceMock.Setup(p => p.GetFollowers(It.IsAny<Func<Follower, bool>>()))
                    .Returns(Task.FromResult(expectedListOfFollowers)).Verifiable();

                var target = mock.CreateInstance<FollowerDomainService>();

                //Act
                var result = await target.GetFollowedUsers(username);

                //Assert
                result.Should().BeEquivalentTo(expectedListOfFollowers);
                persistenceServiceMock.Verify(x => x.GetUsers(It.IsAny<Func<User, bool>>()));
                persistenceServiceMock.Verify(x => x.GetFollowers(It.IsAny<Func<Follower, bool>>()));
            }

            [Test]
            public async Task GetFollowedUsers_When_ValidUsernameProvided_Then_ReturnsListOfFollowers()
            {
                //Arrange
                var mock = new AutoMocker();
                var username = "name";
                var id = 1;
                var numberOfRows = 2;
                IEnumerable<User> persistenceUser = new List<User> { new User { UserId = id } };
                IEnumerable<Follower> expectedListOfFollowers = new List<Follower>
                {
                    new Follower
                    {
                        Id = 1,
                        WhoId = 1,
                        WhomId = id,
                    },
                    new Follower
                    {
                        Id = 2,
                        WhoId = 2,
                        WhomId = id,
                    },
                    new Follower
                    {
                        Id = 3,
                        WhoId = 3,
                        WhomId = id,
                    }
                };

                var persistenceServiceMock = mock.GetMock<IPersistenceService>();
                persistenceServiceMock.Setup(p => p.GetUsers(It.IsAny<Func<User, bool>>()))
                    .Returns(Task.FromResult(persistenceUser)).Verifiable();
                persistenceServiceMock.Setup(p => p.GetFollowers(It.IsAny<Func<Follower, bool>>()))
                    .Returns(Task.FromResult(expectedListOfFollowers)).Verifiable();

                var target = mock.CreateInstance<FollowerDomainService>();

                //Act
                var result = await target.GetFollowedUsers(username, numberOfRows);

                //Assert
                result.Should().BeEquivalentTo(expectedListOfFollowers.Take(numberOfRows));
                result.ToList().Count.Should().Be(numberOfRows);
                persistenceServiceMock.Verify(x => x.GetUsers(It.IsAny<Func<User, bool>>()));
                persistenceServiceMock.Verify(x => x.GetFollowers(It.IsAny<Func<Follower, bool>>()));
            }

            [Test]
            public async Task GetFollowedUsers_When_ValidIdProvided_Then_ReturnsListOfFollowers()
            {
                //Arrange
                var mock = new AutoMocker();
                var id = 5;
                Func<Follower, bool> func = entry => entry.WhoId.Equals(id);
                IEnumerable<Follower> expectedListOfFollowers = new List<Follower>
            {
                new Follower
                {
                    Id = 1,
                    WhoId = 2,
                    WhomId = id,
                }
            };

                var persistenceServiceMock = mock.GetMock<IPersistenceService>();
                persistenceServiceMock.Setup(p => p.GetFollowers(It.IsAny<Func<Follower, bool>>()))
                    .Returns(Task.FromResult(expectedListOfFollowers)).Verifiable();

                var target = mock.CreateInstance<FollowerDomainService>();

                //Act
                var result = await target.GetFollowedUsers(id);

                //Assert
                result.Should().BeEquivalentTo(expectedListOfFollowers);
                persistenceServiceMock.Verify(x => x.GetFollowers(It.IsAny<Func<Follower, bool>>()));
            }

            [Test]
            public async Task GetFollowedUsers_When_ValidIdProvidedAndNumberOfRows_Then_ReturnsListOfFollowers()
            {
                //Arrange
                var mock = new AutoMocker();
                var id = 5;
                var numberOfRows = 2;
                Func<Follower, bool> func = entry => entry.WhoId.Equals(id);
                IEnumerable<Follower> expectedListOfFollowers = new List<Follower>
            {
                new Follower
                {
                    Id = 1,
                    WhoId = 1,
                    WhomId = id,
                },
                new Follower
                {
                    Id = 2,
                    WhoId = 2,
                    WhomId = id,
                },
                new Follower
                {
                    Id = 3,
                    WhoId = 3,
                    WhomId = id,
                }
            };

                var persistenceServiceMock = mock.GetMock<IPersistenceService>();
                persistenceServiceMock.Setup(p => p.GetFollowers(It.IsAny<Func<Follower, bool>>()))
                    .Returns(Task.FromResult(expectedListOfFollowers)).Verifiable();

                var target = mock.CreateInstance<FollowerDomainService>();

                //Act
                var result = await target.GetFollowedUsers(id, numberOfRows);

                //Assert
                result.Should().BeEquivalentTo(expectedListOfFollowers.Take(numberOfRows));
                result.ToList().Count.Should().Be(numberOfRows);
                persistenceServiceMock.Verify(x => x.GetFollowers(It.IsAny<Func<Follower, bool>>()));
            }
        }
        
        class FollowTest
        {
            [Test]
            public async Task Follow_When_ValidFollowerProvided_Then_Returns()
            {
                //Arrange
                var mock = new AutoMocker();
                var id = 5;
                var follower = new Follower
                {
                    WhoId = id,
                    WhomId = 6
                };
                IEnumerable<User> persistenceUser = new List<User> { new User { UserId = id } };
                IEnumerable<Follower> expectedListOfFollowers = new List<Follower>
                {
                    new Follower
                    {
                        Id = 1,
                        WhoId = 1,
                        WhomId = 2,
                    },
                    new Follower
                    {
                        Id = 2,
                        WhoId = 2,
                        WhomId = 3,
                    },
                    new Follower
                    {
                        Id = 3,
                        WhoId = 3,
                        WhomId = 4,
                    }
                };

                var persistenceServiceMock = mock.GetMock<IPersistenceService>();
                persistenceServiceMock.Setup(p => p.GetUsers(It.IsAny<Func<User, bool>>()))
                    .Returns(Task.FromResult(persistenceUser)).Verifiable();
                persistenceServiceMock.Setup(p => p.GetFollowers(It.IsAny<Func<Follower, bool>>()))
                    .Returns(Task.FromResult(expectedListOfFollowers)).Verifiable();

                var target = mock.CreateInstance<FollowerDomainService>();

                //Act
                await target.Follow(follower);

                //Assert
                persistenceServiceMock.Verify(x => x.GetUsers(It.IsAny<Func<User, bool>>()));
                persistenceServiceMock.Verify(x => x.GetFollowers(It.IsAny<Func<Follower, bool>>()));
                persistenceServiceMock.Verify(x => x.AddFollower(follower));
            }

            [Test]
            public async Task Follow_When_ValidUsernamesProvided_Then_Returns()
            {
                //Arrange
                var mock = new AutoMocker();
                var userNameWho = "who";
                var userNameWhom = "whom";
                var whoId = 5;
                var whomId = 6;
                IEnumerable<User> persistenceUserWho = new List<User> { new User { UserId = whoId } };
                IEnumerable<User> persistenceUserWhom = new List<User> { new User { UserId = whomId } };
                IEnumerable<Follower> expectedListOfFollowers = new List<Follower>
                {
                    new Follower
                    {
                        Id = 1,
                        WhoId = 1,
                        WhomId = 2,
                    },
                    new Follower
                    {
                        Id = 2,
                        WhoId = 2,
                        WhomId = 3,
                    },
                    new Follower
                    {
                        Id = 3,
                        WhoId = 3,
                        WhomId = 4,
                    }
                };

                var persistenceServiceMock = mock.GetMock<IPersistenceService>();
                persistenceServiceMock.SetupSequence(p => p.GetUsers(It.IsAny<Func<User, bool>>()))
                    .Returns(Task.FromResult(persistenceUserWho))
                    .Returns(Task.FromResult(persistenceUserWhom));

                var target = mock.CreateInstance<FollowerDomainService>();

                //Act
                await target.Follow(userNameWho, userNameWhom);

                //Assert
                persistenceServiceMock.Verify(x => x.GetUsers(It.IsAny<Func<User, bool>>()), Times.Exactly(2));
                persistenceServiceMock.Verify(x => x.AddFollower(It.Is<Follower>(r => r.WhomId.Equals(whomId) && r.WhoId.Equals(whoId))));
            }
        }

        class UnfollowTest
        {
            //[Test]
            //public async Task UnFollow_When_ValidUnFollowerProvided_Then_Returns()
            //{
            //    //Arrange
            //    var mock = new AutoMocker();
            //    var id = 5;
            //    var follower = new Follower
            //    {
            //        WhoId = id,
            //        WhomId = 6
            //    };
            //    IEnumerable<User> persistenceUser = new List<User> { new User { UserId = id } };
            //    IEnumerable<Follower> expectedListOfFollowers = new List<Follower>
            //    {
            //        new Follower
            //        {
            //            Id = 1,
            //            WhoId = 1,
            //            WhomId = 2,
            //        },
            //        new Follower
            //        {
            //            Id = 2,
            //            WhoId = 2,
            //            WhomId = 3,
            //        },
            //        new Follower
            //        {
            //            Id = 3,
            //            WhoId = 3,
            //            WhomId = 4,
            //        }
            //    };

            //    var persistenceServiceMock = mock.GetMock<IPersistenceService>();
            //    persistenceServiceMock.Setup(p => p.GetUsers(It.IsAny<Func<User, bool>>()))
            //        .Returns(Task.FromResult(persistenceUser)).Verifiable();
            //    persistenceServiceMock.Setup(p => p.GetFollowers(It.IsAny<Func<Follower, bool>>()))
            //        .Returns(Task.FromResult(expectedListOfFollowers)).Verifiable();

            //    var target = mock.CreateInstance<FollowerDomainService>();

            //    //Act
            //    await target.UnFollow(id);

            //    //Assert
            //    persistenceServiceMock.Verify(x => x.GetUsers(It.IsAny<Func<User, bool>>()));
            //    persistenceServiceMock.Verify(x => x.GetFollowers(It.IsAny<Func<Follower, bool>>()));
            //    persistenceServiceMock.Verify(x => x.DeleteFollower(follower));
            //}

            [Test]
            public async Task Follow_When_ValidUsernamesProvided_Then_Returns()
            {
                //Arrange
                var mock = new AutoMocker();
                var userNameWho = "who";
                var userNameWhom = "whom";
                var whoId = 5;
                var whomId = 6;
                IEnumerable<User> persistenceUserWho = new List<User> { new User { UserId = whoId } };
                IEnumerable<User> persistenceUserWhom = new List<User> { new User { UserId = whomId } };
                IEnumerable<Follower> expectedListOfFollowers = new List<Follower>
                {
                    new Follower
                    {
                        Id = 1,
                        WhoId = whoId,
                        WhomId = whomId,
                    }
                };

                var persistenceServiceMock = mock.GetMock<IPersistenceService>();
                persistenceServiceMock.SetupSequence(p => p.GetUsers(It.IsAny<Func<User, bool>>()))
                    .Returns(Task.FromResult(persistenceUserWho))
                    .Returns(Task.FromResult(persistenceUserWhom));
                persistenceServiceMock.Setup(p => p.GetFollowers(It.IsAny<Func<Follower, bool>>()))
                    .Returns(Task.FromResult(expectedListOfFollowers)).Verifiable();

                var target = mock.CreateInstance<FollowerDomainService>();

                //Act
                await target.UnFollow(userNameWho, userNameWhom);

                //Assert
                persistenceServiceMock.Verify(x => x.GetUsers(It.IsAny<Func<User, bool>>()), Times.Exactly(2));
                persistenceServiceMock.Verify(x => x.DeleteFollower(It.Is<Follower>(r => r.WhomId.Equals(whomId) && r.WhoId.Equals(whoId))));
            }
        }
    }
}