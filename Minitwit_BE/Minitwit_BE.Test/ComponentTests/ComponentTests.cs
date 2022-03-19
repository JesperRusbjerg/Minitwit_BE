using Minitwit_BE.Api.Controllers.Simulator;
using Minitwit_BE.Api.Dtos.Simulation;
using Minitwit_BE.Domain;
using Minitwit_BE.DomainService;
using Minitwit_BE.DomainService.Interfaces;
using Minitwit_BE.Persistence;
using Moq;
using Moq.AutoMock;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using System.Linq;

namespace Minitwit_BE.Test
{
    public class ComponentTests
    {
        [Test]
        public async Task Latest()
        {
            //Arrange
            var mock = new AutoMocker();
            var latestValue = 5;

            var simulatorServiceMock = mock.CreateInstance<SimulatorService>();
            mock.Use<ISimulationService>(simulatorServiceMock);

            var persistenceServiceMock = mock.GetMock<IPersistenceService>();
            persistenceServiceMock.Setup(p => p.GetLatest())
                .Returns(Task.FromResult(latestValue)).Verifiable();

            var target = mock.CreateInstance<SimulatorController>();

            //Act
            var result = await target.Latest();

            //Assert
            Assert.AreEqual(latestValue, result.Value.Latest);
            persistenceServiceMock.Verify(x => x.GetLatest(), Times.Once);
        }

        [Test]
        public async Task Register_When_ValidRegisterDtoProvided_And_UserDoesNotExistInDB()
        {
            //Arrange
            var mock = new AutoMocker();
            var latestValue = 5;
            var registerDto = new RegisterDto
            {
                Email = "e@e.dk",
                Password = "p",
                UserName = "n"
            };
            IEnumerable<User> persistenceUsers = new List<User>();

            var simulatorServiceMock = mock.CreateInstance<SimulatorService>();
            mock.Use<ISimulationService>(simulatorServiceMock);

            var userServiceMock = mock.CreateInstance<UserDomainService>();
            mock.Use<IUserDomainService>(userServiceMock);

            var persistenceServiceMock = mock.GetMock<IPersistenceService>();
            persistenceServiceMock.Setup(p => p.UpdateLatest(It.IsAny<int>())).Verifiable();
            persistenceServiceMock.Setup(p => p.GetUsers(It.IsAny<Func<User, bool>>()))
                .Returns(Task.FromResult(persistenceUsers)).Verifiable();
            
            var target = mock.CreateInstance<SimulatorController>();

            //Act
            var result = await target.Register(registerDto, latestValue);

            //Assert
            persistenceServiceMock.Verify(x => x.UpdateLatest(latestValue), Times.Once);
            persistenceServiceMock.Verify(x => x.AddUser(It.Is<User>(u => 
                u.UserName.Equals(registerDto.UserName) &&
                u.Email.Equals(registerDto.Email))), Times.Once);
        }

        [Test]
        public async Task GetPublicMessages_When_ValidNumberProvided()
        {
            //Arrange
            var mock = new AutoMocker();
            var latestValue = 5;
            var numberOfMsg = 2;
            var id = 1;
            var messages = TestHelper.GetMessages();
            var user = new User
            {
                UserName = "n",
                UserId = id,
            };

            var simulatorServiceMock = mock.CreateInstance<SimulatorService>();
            mock.Use<ISimulationService>(simulatorServiceMock);

            var userServiceMock = mock.GetMock<IUserDomainService>();
            userServiceMock.Setup(p => p.GetUserById(It.IsAny<int>()))
                .Returns(Task.FromResult(user)).Verifiable();

            var messageServiceMock = mock.CreateInstance<MessageDomainService>();
            mock.Use<IMessageDomainService>(messageServiceMock);

            var persistenceServiceMock = mock.GetMock<IPersistenceService>();
            persistenceServiceMock.Setup(p => p.UpdateLatest(It.IsAny<int>())).Verifiable();
            persistenceServiceMock.Setup(p => p.GetMessages(It.IsAny<Func<Message, bool>>()))
                .Returns(Task.FromResult(messages)).Verifiable();

            var target = mock.CreateInstance<SimulatorController>();
            target.ControllerContext = TestHelper.CreateHttpContext();

            //Act
            var result = ((await target.GetPublicMessages(latestValue, numberOfMsg)).Result as OkObjectResult).Value as IEnumerable<GetMessageDto>;
            var resultList = result.ToList();

            //Assert
            persistenceServiceMock.Verify(x => x.UpdateLatest(latestValue), Times.Once);
            persistenceServiceMock.Verify(x => x.GetMessages(It.IsAny<Func<Message, bool>>()), Times.Once);

            for (int i = 0; i < resultList.Count(); i++)
            {
                Assert.AreEqual(resultList[i].UserName, user.UserName);
                Assert.AreEqual(resultList[i].Text, messages.ToList()[i].Text);
                Assert.AreEqual(resultList[i].PublishDate, messages.ToList()[i].PublishDate.ToString());
            }
        }

        [Test]
        public async Task GetPersonalMessages_When_ValidUsernameAndNumberProvided()
        {
            //Arrange
            var mock = new AutoMocker();
            var latestValue = 5;
            var numberOfMsg = 2;
            var id = 1;
            var messages = TestHelper.GetMessagesWithUserId(id);
            var user = new User
            {
                UserName = "name",
                UserId = id,
            };
            IEnumerable<User> persistenceUsers = new List<User> { user };

            var simulatorServiceMock = mock.CreateInstance<SimulatorService>();
            mock.Use<ISimulationService>(simulatorServiceMock);

            var messageServiceMock = mock.CreateInstance<MessageDomainService>();
            mock.Use<IMessageDomainService>(messageServiceMock);

            var persistenceServiceMock = mock.GetMock<IPersistenceService>();
            persistenceServiceMock.Setup(p => p.UpdateLatest(It.IsAny<int>())).Verifiable();
            persistenceServiceMock.Setup(p => p.GetMessages(It.IsAny<Func<Message, bool>>()))
                .Returns(Task.FromResult(messages)).Verifiable();
            persistenceServiceMock.Setup(p => p.GetUsers(It.IsAny<Func<User, bool>>()))
                .Returns(Task.FromResult(persistenceUsers)).Verifiable();

            var target = mock.CreateInstance<SimulatorController>();
            target.ControllerContext = TestHelper.CreateHttpContext();

            //Act
            var result = ((await target.GetPersonalMessages(user.UserName, latestValue, numberOfMsg)).Result as OkObjectResult).Value as IEnumerable<GetMessageDto>;
            var resultList = result.ToList();

            //Assert
            persistenceServiceMock.Verify(x => x.UpdateLatest(latestValue), Times.Once);
            persistenceServiceMock.Verify(x => x.GetMessages(It.IsAny<Func<Message, bool>>()), Times.Once);
            persistenceServiceMock.Verify(x => x.GetUsers(It.IsAny<Func<User, bool>>()), Times.Once);

            for (int i = 0; i < resultList.Count(); i++)
            {
                Assert.AreEqual(resultList[i].UserName, user.UserName);
                Assert.AreEqual(resultList[i].Text, messages.ToList()[i].Text);
                Assert.AreEqual(resultList[i].PublishDate, messages.ToList()[i].PublishDate.ToString());
            }
        }

        [Test]
        public async Task AddMessage_When_ValidDtoProvided()
        {
            //Arrange
            var mock = new AutoMocker();
            var latestValue = 5;
            var id = 1;
            var dto = new AddMessageDto
            {
                Content = "Text"
            };
            var user = new User
            {
                UserName = "name",
                UserId = id,
            };
            IEnumerable<User> persistenceUsers = new List<User> { user };

            var simulatorServiceMock = mock.CreateInstance<SimulatorService>();
            mock.Use<ISimulationService>(simulatorServiceMock);

            var messageServiceMock = mock.CreateInstance<MessageDomainService>();
            mock.Use<IMessageDomainService>(messageServiceMock);

            var persistenceServiceMock = mock.GetMock<IPersistenceService>();
            persistenceServiceMock.Setup(p => p.UpdateLatest(It.IsAny<int>())).Verifiable();
            persistenceServiceMock.Setup(p => p.GetUsers(It.IsAny<Func<User, bool>>()))
                .Returns(Task.FromResult(persistenceUsers)).Verifiable();

            var target = mock.CreateInstance<SimulatorController>();
            target.ControllerContext = TestHelper.CreateHttpContext();

            //Act
            var result = await target.AddTwit(dto, user.UserName, latestValue);

            //Assert
            persistenceServiceMock.Verify(x => x.UpdateLatest(latestValue), Times.Once);
            persistenceServiceMock.Verify(x => x.AddMessage(It.Is<Message>(m => 
                m.AuthorId.Equals(user.UserId) &&
                m.Flagged.Equals(false) &&
                m.Text.Equals(dto.Content))), Times.Once);
            persistenceServiceMock.Verify(x => x.GetUsers(It.IsAny<Func<User, bool>>()), Times.Once);
        }

        [Test]
        public async Task GetFollowedUsers_When_ValidUsernameProvided()
        {
            //Arrange
            var mock = new AutoMocker();
            var latestValue = 5;
            var numberOfFlws = 2;
            var id = 1;
            var followers = TestHelper.GetFollowersWithWhoUserId(id);
            var user = new User
            {
                UserName = "name",
                UserId = id,
            };
            var userWhom = new User
            {
                UserName = "nameZ",
            };
            IEnumerable<User> persistenceUsers = new List<User> { user };

            var simulatorServiceMock = mock.CreateInstance<SimulatorService>();
            mock.Use<ISimulationService>(simulatorServiceMock);

            var followerServiceMock = mock.CreateInstance<FollowerDomainService>();
            mock.Use<IFollowerDomainService>(followerServiceMock);

            var userServiceMock = mock.GetMock<IUserDomainService>();
            userServiceMock.Setup(p => p.GetUserById(It.IsAny<int>()))
                .Returns(Task.FromResult(userWhom)).Verifiable();

            var persistenceServiceMock = mock.GetMock<IPersistenceService>();
            persistenceServiceMock.Setup(p => p.UpdateLatest(It.IsAny<int>())).Verifiable();
            persistenceServiceMock.Setup(p => p.GetFollowers(It.IsAny<Func<Follower, bool>>()))
                .Returns(Task.FromResult(followers)).Verifiable();
            persistenceServiceMock.Setup(p => p.GetUsers(It.IsAny<Func<User, bool>>()))
                .Returns(Task.FromResult(persistenceUsers)).Verifiable();

            var target = mock.CreateInstance<SimulatorController>();
            target.ControllerContext = TestHelper.CreateHttpContext();

            //Act
            var result = await target.GetFollowedUsers(user.UserName, numberOfFlws, latestValue);
            var listOfFollowsUsernames = ((result.Result as OkObjectResult).Value as FollowsResponseDto).Follows;

            //Assert
            persistenceServiceMock.Verify(x => x.UpdateLatest(latestValue), Times.Once);
            persistenceServiceMock.Verify(x => x.GetFollowers(It.IsAny<Func<Follower, bool>>()), Times.Once);
            persistenceServiceMock.Verify(x => x.GetUsers(It.IsAny<Func<User, bool>>()), Times.Once);

            for (int i = 0; i < listOfFollowsUsernames.Count(); i++)
            {
                Assert.AreEqual(listOfFollowsUsernames[i], userWhom.UserName);
            }
        }

        [Test]
        public async Task Follow()
        {
            //Arrange
            var mock = new AutoMocker();
            var latestValue = 5;
            var id = 1;
            var userToFollow = new User
            {
                UserName = "follow",
                UserId = id+1,
            };
            var dto = new FollowerDtoSimulation
            {
                Follow = userToFollow.UserName
            };
            var user = new User
            {
                UserName = "name",
                UserId = id,
            };
            IEnumerable<User> persistenceUsersWho = new List<User> { user };
            IEnumerable<User> persistenceUsersWhom = new List<User> { userToFollow };

            var simulatorServiceMock = mock.CreateInstance<SimulatorService>();
            mock.Use<ISimulationService>(simulatorServiceMock);

            var followServiceMock = mock.CreateInstance<FollowerDomainService>();
            mock.Use<IFollowerDomainService>(followServiceMock);

            var seq = new MockSequence();
            var persistenceServiceMock = mock.GetMock<IPersistenceService>();
            persistenceServiceMock.InSequence(seq).Setup(p => p.GetUsers(It.IsAny<Func<User, bool>>())).Returns(Task.FromResult(persistenceUsersWho)).Verifiable();
            persistenceServiceMock.InSequence(seq).Setup(p => p.GetUsers(It.IsAny<Func<User, bool>>())).Returns(Task.FromResult(persistenceUsersWhom)).Verifiable();

            var target = mock.CreateInstance<SimulatorController>();
            target.ControllerContext = TestHelper.CreateHttpContext();

            //Act
            var result = await target.FollowOrUnfollowUser(dto, user.UserName, latestValue);

            //Assert
            persistenceServiceMock.Verify(x => x.UpdateLatest(latestValue), Times.Once);
            persistenceServiceMock.Verify(x => x.GetUsers(It.IsAny<Func<User, bool>>()), Times.Exactly(2));
            persistenceServiceMock.Verify(x => x.AddFollower(It.Is<Follower>(f => f.WhoId.Equals(user.UserId) && f.WhomId.Equals(userToFollow.UserId))), Times.Once);
        }

        [Test]
        public async Task UnFollow()
        {
            //Arrange
            var mock = new AutoMocker();
            var latestValue = 5;
            var id = 1;
            var userToUnFollow = new User
            {
                UserName = "follow",
                UserId = id + 1,
            };
            var dto = new FollowerDtoSimulation
            {
                Unfollow = userToUnFollow.UserName
            };
            var user = new User
            {
                UserName = "name",
                UserId = id,
            };
            IEnumerable<User> persistenceUsersWho = new List<User> { user };
            IEnumerable<User> persistenceUsersWhom = new List<User> { userToUnFollow };
            IEnumerable<Follower> persistenceFollows = new List<Follower> { new Follower { WhoId = id, WhomId = id + 1 } };

            var simulatorServiceMock = mock.CreateInstance<SimulatorService>();
            mock.Use<ISimulationService>(simulatorServiceMock);

            var followServiceMock = mock.CreateInstance<FollowerDomainService>();
            mock.Use<IFollowerDomainService>(followServiceMock);

            var seq = new MockSequence();
            var persistenceServiceMock = mock.GetMock<IPersistenceService>();
            persistenceServiceMock.InSequence(seq).Setup(p => p.GetUsers(It.IsAny<Func<User, bool>>())).Returns(Task.FromResult(persistenceUsersWho)).Verifiable();
            persistenceServiceMock.InSequence(seq).Setup(p => p.GetUsers(It.IsAny<Func<User, bool>>())).Returns(Task.FromResult(persistenceUsersWhom)).Verifiable();
            persistenceServiceMock.Setup(p => p.GetFollowers(It.IsAny<Func<Follower, bool>>())).Returns(Task.FromResult(persistenceFollows)).Verifiable();

            var target = mock.CreateInstance<SimulatorController>();
            target.ControllerContext = TestHelper.CreateHttpContext();

            //Act
            var result = await target.FollowOrUnfollowUser(dto, user.UserName, latestValue);

            //Assert
            persistenceServiceMock.Verify(x => x.UpdateLatest(latestValue), Times.Once);
            persistenceServiceMock.Verify(x => x.GetUsers(It.IsAny<Func<User, bool>>()), Times.Exactly(2));
            persistenceServiceMock.Verify(x => x.DeleteFollower(It.Is<Follower>(f => f.WhoId.Equals(user.UserId) && f.WhomId.Equals(userToUnFollow.UserId))), Times.Once);
        }
    }
}