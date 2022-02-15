﻿using Microsoft.AspNetCore.Mvc;
using Minitwit_BE.Api.Dtos.Simulation;
using Minitwit_BE.Domain;
using Minitwit_BE.DomainService.Interfaces;

namespace Minitwit_BE.Api.Controllers.Simulator
{
    [ApiController]
    [Route("simulator")]
    public class SimulatorController : ControllerBase
    {
        private readonly ILogger<SimulatorController> _logger;
        private readonly IMessageDomainService _messageService;
        private readonly IFollowerDomainService _followerService;
        private readonly IUserDomainService _userService;

        public SimulatorController(
            ILogger<SimulatorController> logger, 
            IMessageDomainService messageService,
            IFollowerDomainService followerService,
            IUserDomainService userService)
        {
            _logger = logger;
            _messageService = messageService;
            _followerService = followerService;
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegisterDto input)
        {
            ValidateRegisterDto(input);

            _logger.LogInformation("Register new user in the simulator");

            await _userService.RegisterUser(new User
            {
                UserName = input.UserName,
                Email = input.Email,
                PwHash = input.Password
            });

            return Ok();
        }

        [HttpPost("msgs")]
        public async Task<ActionResult<List<Message>>> GetPublicMessages()
        {
            _logger.LogInformation("Get messages in the simulator");

            // we have to map id of a user into concrete username
            var msgs = await _messageService.GetTwits();

            return Ok(msgs.ToList());
        }


        #region PrivateMethods
        private void ValidateRegisterDto(RegisterDto obj)
        {
            if (string.IsNullOrWhiteSpace(obj.UserName))
                throw new ArgumentException("You have to enter a username");

            if (!obj.Email.Contains("@"))
                throw new ArgumentException("You have to enter a valid email address");

            if (string.IsNullOrWhiteSpace(obj.Password))
                throw new ArgumentException("You have to enter a password");



        }
        #endregion
    }
}
