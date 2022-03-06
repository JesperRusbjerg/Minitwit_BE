using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Minitwit_BE.Api.Dtos;
using Minitwit_BE.Domain;
using Minitwit_BE.DomainService.Interfaces;
using System;
using System.Threading.Tasks;

namespace Minitwit_BE.Api.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserDomainService _userService;

        public UserController(ILogger<UserController> logger, IUserDomainService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> RegisterUser([FromBody] UserDto input)
        {
            ValidateInput(input);

            _logger.LogInformation($"RegisterUser endpoint was called for user: {input.UserName}");

            var user = new User
            {
                UserName = input.UserName,
                Email = input.Email,
                PwHash = input.PwHash,
            };

            await _userService.RegisterUser(user);

            return Ok(user.UserId);
        }

        [HttpPost("login")]
        public async Task<ActionResult<int>> Login([FromBody] UserDto input)
        {
            ValidateInput(input);

            _logger.LogInformation($"Login endpoint was called for user: {input.Email}");

            var user = new User
            {
                UserName = input.UserName,
                Email = input.Email,
                PwHash = input.PwHash,
            };

            int userId = await _userService.Login(user);

            return Ok(userId);
        }

        #region PrivateMethods
        private void ValidateInput(UserDto user)
        {
            if (user == null || string.IsNullOrWhiteSpace(user.PwHash) || string.IsNullOrWhiteSpace(user.Email))
                throw new ArgumentException("Password hash or user email missing!");
        }
        #endregion
    }
}