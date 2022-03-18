using FluentAssertions;
using Minitwit_BE.Domain;
using Minitwit_BE.Domain.Exceptions;
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
    public class UserServiceTest
    {
        [Test]
        public async Task GetUserById_When_ValidIdProvided()
        {
            //Arrange
            var mock = new AutoMocker();
            var id = 1;
            IEnumerable<User> persistenceUser = new List<User> { new User { UserId = id } };

            var persistenceServiceMock = mock.GetMock<IPersistenceService>();
            persistenceServiceMock.Setup(p => p.GetUsers(It.IsAny<Func<User, bool>>()))
                .Returns(Task.FromResult(persistenceUser)).Verifiable();

            var target = mock.CreateInstance<UserDomainService>();

            //Act
            var result = await target.GetUserById(id);

            //Assert
            persistenceServiceMock.Verify(x => x.GetUsers(It.IsAny<Func<User, bool>>()), Times.Once);
            result.Should().BeEquivalentTo(persistenceUser.FirstOrDefault());
        }

        [Test]
        public async Task GetUserById_When_ValidIdProvided_And_NoUserExists_ThrowsException()
        {
            //Arrange
            var mock = new AutoMocker();
            var id = 1;
            IEnumerable<User> persistenceUser = new List<User> { };

            var persistenceServiceMock = mock.GetMock<IPersistenceService>();
            persistenceServiceMock.Setup(p => p.GetUsers(It.IsAny<Func<User, bool>>()))
                .Returns(Task.FromResult(persistenceUser)).Verifiable();

            var target = mock.CreateInstance<UserDomainService>();

            //Act & Assert
            var exception = Assert.ThrowsAsync<ArgumentException>(async () => await target.GetUserById(id));
            Assert.AreEqual(exception.Message, "User does not exist");
            persistenceServiceMock.Verify(x => x.GetUsers(It.IsAny<Func<User, bool>>()), Times.Once);
        }

        [Test]
        public async Task RegisterUser_When_ValidUserProvided()
        {
            //Arrange
            var mock = new AutoMocker();
            var id = 1;
            var user = new User { UserId = id };
            IEnumerable<User> persistenceUser = new List<User> { };

            var persistenceServiceMock = mock.GetMock<IPersistenceService>();
            persistenceServiceMock.Setup(p => p.GetUsers(It.IsAny<Func<User, bool>>()))
                .Returns(Task.FromResult(persistenceUser)).Verifiable();

            var target = mock.CreateInstance<UserDomainService>();

            //Act
            await target.RegisterUser(user);

            //Assert
            persistenceServiceMock.Verify(x => x.GetUsers(It.IsAny<Func<User, bool>>()), Times.Once);
            persistenceServiceMock.Verify(x => x.AddUser(It.Is<User>(r => r.UserId.Equals(user.UserId))), Times.Once);
        }

        [Test]
        public async Task RegisterUser_When_ValidUserProvided_And_UserAlreadyExists()
        {
            //Arrange
            var mock = new AutoMocker();
            var id = 1;
            var user = new User { UserId = id };
            IEnumerable<User> persistenceUser = new List<User> { new User { UserId = id } };

            var persistenceServiceMock = mock.GetMock<IPersistenceService>();
            persistenceServiceMock.Setup(p => p.GetUsers(It.IsAny<Func<User, bool>>()))
                .Returns(Task.FromResult(persistenceUser)).Verifiable();

            var target = mock.CreateInstance<UserDomainService>();

            //Act & Assert
            var exception = Assert.ThrowsAsync<UserAlreadyExistsException>(async () => await target.RegisterUser(user));
            Assert.AreEqual(exception.Message, "The username is already taken");
            persistenceServiceMock.Verify(x => x.GetUsers(It.IsAny<Func<User, bool>>()), Times.Once);
            persistenceServiceMock.Verify(x => x.AddUser(It.Is<User>(r => r.UserId.Equals(user.UserId))), Times.Never);
        }
    }
}