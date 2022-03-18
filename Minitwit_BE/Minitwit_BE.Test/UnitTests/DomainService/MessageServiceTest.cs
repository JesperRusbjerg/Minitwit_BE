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
    public class MessageServiceTest
    {
        [Test]
        public async Task AddTwit_When_ValidMessageAndUsernameProvided_Then_Returns()
        {
            //Arrange
            var mock = new AutoMocker();
            var username = "name";
            var id = 1;
            var msg = new Message { Text = "text", Flagged = false, PublishDate = DateTime.Now };
            IEnumerable<User> persistenceUser = new List<User> { new User { UserId = id } };

            var persistenceServiceMock = mock.GetMock<IPersistenceService>();
            persistenceServiceMock.Setup(p => p.GetUsers(It.IsAny<Func<User, bool>>()))
                .Returns(Task.FromResult(persistenceUser)).Verifiable();

            var target = mock.CreateInstance<MessageDomainService>();

            //Act
            await target.AddTwit(msg, username);

            //Assert
            persistenceServiceMock.Verify(x => x.GetUsers(It.IsAny<Func<User, bool>>()), Times.Once);
            persistenceServiceMock.Verify(x => x.AddMessage(It.Is<Message>(r => 
                r.AuthorId.Equals(id) && 
                r.Text.Equals(msg.Text) &&
                r.PublishDate.Equals(msg.PublishDate))), Times.Once());
        }

        [Test]
        public async Task GetTwits_Then_ReturnsMessages()
        {
            //Arrange
            var mock = new AutoMocker();
            IEnumerable<Message> messages = new List<Message> 
            {
                new Message
                {
                    AuthorId = 1,
                    Flagged = false,
                    MessageId = 1,
                    PublishDate = DateTime.Now,
                    Text = "text"
                },
                new Message
                {
                    AuthorId = 1,
                    Flagged = false,
                    MessageId = 2,
                    PublishDate = DateTime.Now,
                    Text = "text"
                }
            };

            var persistenceServiceMock = mock.GetMock<IPersistenceService>();
            persistenceServiceMock.Setup(p => p.GetMessages(It.IsAny<Func<Message, bool>>()))
                .Returns(Task.FromResult(messages)).Verifiable();

            var target = mock.CreateInstance<MessageDomainService>();

            //Act
            var result = await target.GetTwits();

            //Assert
            result.Should().BeEquivalentTo(messages);
            persistenceServiceMock.Verify(x => x.GetMessages(It.IsAny<Func<Message, bool>>()), Times.Once);
        }

        [Test]
        public async Task GetTwits_WhenNumberOfRowsProvided_Then_ReturnsMessages()
        {
            //Arrange
            var mock = new AutoMocker();
            var numberOfRows = 1;
            IEnumerable<Message> messages = new List<Message>
            {
                new Message
                {
                    AuthorId = 1,
                    Flagged = false,
                    MessageId = 1,
                    PublishDate = DateTime.Now,
                    Text = "text"
                },
                new Message
                {
                    AuthorId = 1,
                    Flagged = false,
                    MessageId = 2,
                    PublishDate = DateTime.Now,
                    Text = "text"
                }
            };

            var persistenceServiceMock = mock.GetMock<IPersistenceService>();
            persistenceServiceMock.Setup(p => p.GetMessages(It.IsAny<Func<Message, bool>>()))
                .Returns(Task.FromResult(messages)).Verifiable();

            var target = mock.CreateInstance<MessageDomainService>();

            //Act
            var result = await target.GetTwits(numberOfRows);

            //Assert
            result.Should().BeEquivalentTo(messages.Take(1));
            result.ToList().Count.Should().Be(numberOfRows);
            persistenceServiceMock.Verify(x => x.GetMessages(It.IsAny<Func<Message, bool>>()), Times.Once);
        }

        [Test]
        public async Task GetPersonalTwits_When_ValidUsernameProvided_Then_ReturnsMessages()
        {
            //Arrange
            var mock = new AutoMocker();
            var numberOfRows = 1;
            var username = "name";
            var id = 1;
            IEnumerable<User> persistenceUser = new List<User> { new User { UserId = id } };
            IEnumerable<Message> messages = new List<Message>
            {
                new Message
                {
                    AuthorId = 1,
                    Flagged = false,
                    MessageId = 1,
                    PublishDate = DateTime.Now,
                    Text = "text"
                },
                new Message
                {
                    AuthorId = 1,
                    Flagged = false,
                    MessageId = 2,
                    PublishDate = DateTime.Now,
                    Text = "text"
                }
            };

            var persistenceServiceMock = mock.GetMock<IPersistenceService>();
            persistenceServiceMock.Setup(p => p.GetMessages(It.IsAny<Func<Message, bool>>()))
                .Returns(Task.FromResult(messages)).Verifiable();
            persistenceServiceMock.Setup(p => p.GetUsers(It.IsAny<Func<User, bool>>()))
                .Returns(Task.FromResult(persistenceUser)).Verifiable();

            var target = mock.CreateInstance<MessageDomainService>();

            //Act
            var result = await target.GetPersonalTwits(username);

            //Assert
            result.Should().BeEquivalentTo(messages);
            persistenceServiceMock.Verify(x => x.GetMessages(It.IsAny<Func<Message, bool>>()), Times.Once);
        }

        [Test]
        public async Task GetPersonalTwits_When_ValidUsernameAndNumberOfUsersProvided_Then_ReturnsMessages()
        {
            //Arrange
            var mock = new AutoMocker();
            var numberOfRows = 1;
            var username = "name";
            var id = 1;
            IEnumerable<User> persistenceUser = new List<User> { new User { UserId = id } };
            IEnumerable<Message> messages = new List<Message>
            {
                new Message
                {
                    AuthorId = 1,
                    Flagged = false,
                    MessageId = 1,
                    PublishDate = DateTime.Now,
                    Text = "text"
                },
                new Message
                {
                    AuthorId = 2,
                    Flagged = false,
                    MessageId = 2,
                    PublishDate = DateTime.Now,
                    Text = "text"
                }
            };

            var persistenceServiceMock = mock.GetMock<IPersistenceService>();
            persistenceServiceMock.Setup(p => p.GetUsers(It.IsAny<Func<User, bool>>()))
                .Returns(Task.FromResult(persistenceUser)).Verifiable();
            persistenceServiceMock.Setup(p => p.GetMessages(It.IsAny<Func<Message, bool>>()))
                .Returns(Task.FromResult(messages)).Verifiable();

            var target = mock.CreateInstance<MessageDomainService>();

            //Act
            var result = await target.GetPersonalTwits(username, numberOfRows);

            //Assert
            result.Should().BeEquivalentTo(messages.Take(numberOfRows));
            result.ToList().Count().Should().Be(numberOfRows);
            persistenceServiceMock.Verify(x => x.GetMessages(It.IsAny<Func<Message, bool>>()), Times.Once);
        }
    }
}