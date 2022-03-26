using Microsoft.AspNetCore.Mvc;
using Minitwit_BE.Api.Dtos;
using Minitwit_BE.Domain;
using Minitwit_BE.DomainService.Interfaces;
using Serilog.Context;

namespace Minitwit_BE.Api.Controllers
{
    [ApiController]
    [Route("api/twit")]
    public class TwitController : ControllerBase
    {
        private readonly ILogger<TwitController> _logger;
        private readonly IMessageDomainService _messageService;
        private readonly IUserDomainService _userDomainService;

        public TwitController(ILogger<TwitController> logger, IMessageDomainService messageService, IUserDomainService userDomainService)
        {
            _logger = logger;
            _messageService = messageService;
            _userDomainService = userDomainService;
        }

        [HttpGet("test")]
        public Task<string> TestEndpoint()
        {
            using (LogContext.PushProperty("testProp", "testValue"))
            {
                _logger.LogInformation("Test message!");
            }

            return Task.FromResult("test4");
        }


        [HttpPost("add")]
        public async Task<ActionResult> AddTwit([FromBody] MessageDto input)
        {
            ValidateMessageDto(input);

            _logger.LogInformation("Inserting a new twit.");

            var msg = new Message                                       // Primary keys should be auto incremented when you add entity to the table and dont explicitely specify specify the ID
            { 
                AuthorId = (int)input.AuthorId,
                Flagged = false,
                PublishDate = DateTime.Now,
                Text = input.Text
            };

            await _messageService.AddTwit(msg);

            return Ok();
        }

        [HttpGet("public-twits")]
        public async Task<ActionResult<List<Message>>> GetTwits([FromQuery(Name = "page")] int? page, [FromQuery(Name = "pageSize")] int? pageSize, [FromQuery] int? no)
        {
            _logger.LogInformation("Returning all public twits");

            var twits = await _messageService.GetTwits(no);

            if (page != null && pageSize != null)
            {

                int startIndex = (int)(pageSize * (page - 1));

                int endIndex = (int)(startIndex + pageSize);

               MessageDtoHack msg = new MessageDtoHack();

                List<Message> messages = twits.ToList();

                if (endIndex >= messages.Count)
                {
                    messages = messages.GetRange(startIndex, messages.Count - startIndex);
                }
                else
                {
                    messages = twits.ToList().GetRange(startIndex, endIndex);
                }

                for (int i = 0; i < messages.Count; i++)
                {
                    User user = await _userDomainService.GetUserById(messages.ElementAt(i).AuthorId);
                    MessageAndUser mau = new MessageAndUser();
                    UserDto usrDto = new UserDto();
                    usrDto.Email = user.Email;
                    usrDto.UserName = user.UserName;
                    usrDto.Email = user.Email;
                    mau.user = usrDto; 
                    mau.msg = messages.ElementAt(i);
                    msg.tweets.Add(mau);
                }

                msg.page = page;

                msg.totalPages = (twits.Count() / pageSize) + 1;

                return Ok(msg);
            }
            else
            {
                return Ok(twits.ToList());
            }
        }

        [HttpGet("personal-twits/{id}")]
        public async Task<ActionResult<List<Message>>> GetPersonalTwits([FromRoute] int id)
        {
            ValidateId(id);
            
            _logger.LogInformation($"Returning all personal twits for user {id}.");

            var user = await _userDomainService.GetUserById(id);
            var twits = await _messageService.GetPersonalTwits(id);

            var userAndPersonalTwits = new UserAndPersonalTwitsDto
            {
                User = new User
                {
                    Email = user.Email,
                    UserName = user.UserName
                },
                Twits = twits.ToList()
            };

            return Ok(userAndPersonalTwits);
        }

        [HttpPut("mark-message")]
        public async Task<ActionResult> MarkMessage([FromBody] FlaggingDto input)
        {
            ValidateFlaggingDto(input);

            _logger.LogInformation($"Mark message endpoint was called on msg id: {input.MessageId}.");

            await _messageService.MarkMessage(input.MessageId, input.FlagMessage);

            return Ok();
        }

        private class MessageDtoHack
        {
            public List<MessageAndUser>? tweets = new List<MessageAndUser>();
            public int? page { get; set; }

            public int? totalPages { get; set; }
        }

        private class MessageAndUser
        {
            public Message msg { get; set; }
            public UserDto user { get; set; }
        }

        private class UserAndPersonalTwitsDto
        {
            public User User { get; set; }
            public List<Message> Twits { get; set; }
        }

        #region PrivateMethods
        private void ValidateFlaggingDto(FlaggingDto input)
        {
            if (input == null || input.MessageId < 0)
            {
                throw new ArgumentException("Input missing or not matching criteria");
            }
        }

        private void ValidateId(int id)
        {
            if (id < 0)
            {
                throw new ArgumentException("Id cannot be negative");
            }
        }

        private void ValidateMessageDto(MessageDto msg)
        {
            if (msg == null || msg.AuthorId < 0 || string.IsNullOrWhiteSpace(msg.Text))
            {
                throw new ArgumentException("Input missing or not matching criteria");
            }

        }
        #endregion
    }
}
