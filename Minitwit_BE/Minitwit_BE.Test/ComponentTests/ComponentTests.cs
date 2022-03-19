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
            var result = await target.GetPublicMessages(latestValue, numberOfMsg);
            //var s = result.Result.Value;

            //Assert
            persistenceServiceMock.Verify(x => x.UpdateLatest(latestValue), Times.Once);
            persistenceServiceMock.Verify(x => x.GetMessages(It.IsAny<Func<Message, bool>>()), Times.Once);
            result.Result.Should().BeEquivalentTo(messages.Take(numberOfMsg));
        }
    }
}