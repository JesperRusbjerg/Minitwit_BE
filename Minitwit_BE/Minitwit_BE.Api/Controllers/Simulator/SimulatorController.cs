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
        public async Task<ActionResult> Register([FromBody] RegisterDto input, [FromQuery] int? latest)
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

            return NoContent();
        }

        [HttpGet("msgs")]
        public async Task<ActionResult<List<Message>>> GetPublicMessages([FromQuery] int? latest, [FromQuery] int? no)
        {
            _logger.LogInformation("Get messages in the simulator");
            await _simulatorService.UpdateLatest(latest);

            var msgs = await _messageService.GetTwits(no);

            var messageDtoTasks = MapMessagesToGetMessageDtos(msgs.ToList());

            await Task.WhenAll(messageDtoTasks);

            return Ok(messageDtoTasks.Select(mTask => mTask.Result));
        }

        [HttpGet("msgs/{username}")]
        public async Task<ActionResult<List<Message>>> GetPersonalMessages([FromRoute] string username, [FromQuery] int? latest, [FromQuery] int? no)
        {
            _logger.LogInformation("Get personal messages in the simulator");
            await _simulatorService.UpdateLatest(latest);

            var msgs = await _messageService.GetPersonalTwits(username, no);

             var messageDtos = msgs.Select(m => new GetMessageDto
            {
                Text = m.Text,
                PublishDate = m.PublishDate.ToString(),
                UserName = username
            });

            if (!messageDtos.ToList().Any())
            {
                return NoContent();
            } else
            {
                return Ok(messageDtos.ToList());
            }
        }

        [HttpPost("msgs/{username}")]
        public async Task<ActionResult> AddTwit([FromBody] AddMessageDto input, [FromRoute] string username, [FromQuery] int? latest)
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

            return NoContent();
        }

        [HttpGet("fllws/{username}")]
        public async Task<ActionResult<List<FollowedUserDto>>> GetFollowedUsers([FromRoute] string username, [FromQuery] int? no, [FromQuery] int? latest)
        {
            _logger.LogInformation($"Follow endpoint was called with username: {username}");
            await _simulatorService.UpdateLatest(latest);

            var followedUsers = await _followerService.GetFollowedUsers(username, no);

            var followedUserDtoTask = MapFollowersToFollowedUserDto(followedUsers.ToList());

            await Task.WhenAll(followedUserDtoTask);

            var followedUserDtos = followedUserDtoTask.Select(f => (f.Result).UserName);

            var follows = new FollowsResponseDto
            {
                Follows = followedUserDtos.ToList()
            };

            return Ok(follows);
        }

        [HttpPost("fllws/{username}")]
        public async Task<ActionResult> FollowOrUnfollowUser([FromBody] FollowerDtoSimulation input, [FromRoute] string username, int? latest)
        {
            _logger.LogInformation(
                $"Follow endpoint was called with username: {username}");
            await _simulatorService.UpdateLatest(latest);

            if (input.Follow != null)
            {
                var followResult = await _followerService.Follow(username, input.Follow);
                if (followResult == 1)
                {
                    return StatusCode(404);
                }
            }
            else if (input.Unfollow != null)
            {
                var unfollowResult = await _followerService.UnFollow(username, input.Unfollow);
                if (unfollowResult == 1)
                {
                   return StatusCode(404);
                }
            }

            return NoContent();
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

        private List<Task<GetMessageDto>> MapMessagesToGetMessageDtos(List<Message> messages)
        {
            return messages.Select(async m => { 
                var user = (await _userService.GetUserById(m.AuthorId));
                return new GetMessageDto
                {
                    Text = m.Text,
                    PublishDate = m.PublishDate.ToString(),
                    UserName = user.UserName
                };
            }).ToList();
        }

        private List<Task<FollowedUserDto>> MapFollowersToFollowedUserDto(List<Follower> followers)
        {
            return followers.Select(async f => { 
                var user = (await _userService.GetUserById(f.WhomId));
                return new FollowedUserDto
                {
                    UserName = user.UserName
                };
            }).ToList();
        }
        #endregion
    }
}
