using Microsoft.AspNetCore.Mvc;
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
        private readonly ISimulationService _simulatorService;

        public SimulatorController(ILogger<SimulatorController> logger,
                                   IMessageDomainService messageService,
                                   IFollowerDomainService followerService,
                                   IUserDomainService userService,
                                   ISimulationService simulatorService)
        {
            _logger = logger;
            _messageService = messageService;
            _followerService = followerService;
            _userService = userService;
            _simulatorService = simulatorService;
        }

        [HttpGet("latest")]
        public async Task<ActionResult<LatestResponse>> Latest()
        {
            int latest = await _simulatorService.GetLatest();
            return new LatestResponse { Latest = latest };
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegisterDto input,
                                                 int latest)
        {
            await _simulatorService.UpdateLatest(latest);
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

        [HttpGet("msgs")]
        public async Task<ActionResult<List<Message>>> GetPublicMessages(int latest)
        {
            _logger.LogInformation("Get messages in the simulator");
            await _simulatorService.UpdateLatest(latest);

            // we have to map id of a user into concrete username
            var msgs = await _messageService.GetTwits();

            return Ok(msgs.ToList());
        }

        [HttpGet("msgs/{username}")]
        public async Task<ActionResult<List<Message>>>
        GetPersonalMessages([FromRoute] string username, int latest)
        {
            _logger.LogInformation("Get personal messages in the simulator");
            await _simulatorService.UpdateLatest(latest);
            // we have to map id of a user into concrete username
            var msgs = await _messageService.GetPersonalTwits(username);

            return Ok(msgs.ToList());
        }

        [HttpPost("msgs/{username}")]
        public async Task<ActionResult> AddTwit([FromBody] AddMessageDto input,
                                                [FromRoute] string username, int latest)
        {
            _logger.LogInformation("Inserting a new twit.");
            await _simulatorService.UpdateLatest(latest);

            var msg = new Message
            {
                Flagged = false,
                PublishDate = DateTime.Now,
                Text = input.Text
            };

            await _messageService.AddTwit(msg, username);

            return Ok();
        }

        [HttpGet("fllws/{username}")]
        public async Task<ActionResult<List<Follower>>>
        GetFollowedUsers([FromRoute] string username, int latest)
        {
            _logger.LogInformation(
                $"Follow endpoint was called with username: {username}");
            await _simulatorService.UpdateLatest(latest);

            var followedUsers = await _followerService.GetFollowedUsers(username);

            return Ok(followedUsers.ToList());
        }

        [HttpPost("fllws/{username}")]
        public async Task<ActionResult>
        FollowOrUnfollowUser([FromBody] FollowerDtoSimulation input,
                             [FromRoute] string username, int latest)
        {
            _logger.LogInformation(
                $"Follow endpoint was called with username: {username}");
            await _simulatorService.UpdateLatest(latest);

            if (input.Follow != null)
            {
                await _followerService.Follow(username, input.Follow);
            }
            else if (input.Unfollow != null)
            {
                await _followerService.UnFollow(username, input.Unfollow);
            }

            return Ok();
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
