using Microsoft.AspNetCore.Mvc;
using Minitwit_BE.Api.Dtos.Simulation;
using Minitwit_BE.Domain;
using Minitwit_BE.Domain.Exceptions;
using Minitwit_BE.DomainService.Interfaces;
using System.Net.Http.Headers;

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

        private bool checkAuthorization(IEnumerable<string> headers)
        {
            return false;
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
            
            //400 if not valid
            ValidateRegisterDto(input);

            _logger.LogInformation("Register new user in the simulator");

            //404 if user already exists
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
            HeaderChecker(Request);

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
            HeaderChecker(Request);

            _logger.LogInformation("Get personal messages in the simulator");
            await _simulatorService.UpdateLatest(latest);

            //404 if user dosent exist
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
            HeaderChecker(Request);

            //400 if wrong input
            ValidateAddMsgDto(input);

            _logger.LogInformation("Inserting a new twit.");
            await _simulatorService.UpdateLatest(latest);

            var msg = new Message
            {
                Flagged = false,
                PublishDate = DateTime.Now,
                Text = input.Content
            };

            await _messageService.AddTwit(msg, username);

            return NoContent();
        }

        [HttpGet("fllws/{username}")]
        public async Task<ActionResult<List<FollowedUserDto>>> GetFollowedUsers([FromRoute] string username, [FromQuery] int? no, [FromQuery] int? latest)
        {
            HeaderChecker(Request);

            _logger.LogInformation($"Follow endpoint was called with username: {username}");
            await _simulatorService.UpdateLatest(latest);

            //404 if user dosent exist
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
            HeaderChecker(Request);

            //400 if input isnt valid
            ValidatefllwDto(input);

            _logger.LogInformation($"Follow endpoint was called with username: {username}");
            await _simulatorService.UpdateLatest(latest);

            if (input.Follow != null)
            {
                //throws 404 if either user dosent exist
                await _followerService.Follow(username, input.Follow);
                return NoContent();
            }
            else if (input.Unfollow != null)
            {
                //throws 404 if either user dosent exist
                await _followerService.UnFollow(username, input.Unfollow);
                return NoContent();
            }

            throw new ArgumentException("Bad request");
        }


        private void HeaderChecker(HttpRequest request)
        {
            var headers = Request.Headers["Authorization"];
            if (headers != "Basic c2ltdWxhdG9yOnN1cGVyX3NhZmUh")
            {
                throw new UnauthorizedException("Unauthorized request");
            }
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

        private void ValidateAddMsgDto(AddMessageDto obj)
        {
            if (string.IsNullOrWhiteSpace(obj.Content))
                throw new ArgumentException("You must add a message");

        }
        private void ValidatefllwDto(FollowerDtoSimulation obj)
        {
            if (string.IsNullOrWhiteSpace(obj.Unfollow) && string.IsNullOrWhiteSpace(obj.Follow))
                throw new ArgumentException("You must specify unfollow or follow");
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
