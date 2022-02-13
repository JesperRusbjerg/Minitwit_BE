using Microsoft.AspNetCore.Mvc;
using Minitwit_BE.Domain;
using Minitwit_BE.Persistence;

namespace Minitwit_BE.Api.Controllers
{
    [ApiController]
    [Route("api/twit")]
    public class TwitController : ControllerBase
    {
        private readonly TwitContext _twitContext;          // To dependency inject the context instance.
        private readonly ILogger<TwitController> _logger;
        
        public TwitController(TwitContext twitContext, ILogger<TwitController> logger)
        {
            _twitContext = twitContext;
            _logger = logger;
        }
        
        [HttpGet("test")]
        public async Task<string> TestEndpoint()
        {
            _logger.LogInformation("Test endpoint was called!");
            
            return await Task.FromResult("test");
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddTwit([FromBody]MessageInput input)
        {
            _logger.LogInformation("Inserting a new twit.");
            

            var msg = new Message                                       // Primary keys should be auto incremented when you add entity to the table and dont explicitely specify specify the ID
            { 
                AuthorId = input.AuthorId,
                Flagged = false,
                PublishDate = DateTime.Now,
                Text = input.Text
            };

            
            _twitContext.Add(msg);
            _twitContext.SaveChanges();

            return Ok();
        }

        [HttpGet("public-twits")]
        public async Task<ActionResult<List<Message>>> GetTwits()
        {

            _logger.LogInformation("Returning all public twits");

            return Ok(_twitContext.Messages.ToList());
        }

        [HttpGet("personal-twits/{id}")]
        public async Task<ActionResult<List<Message>>> GetPersonalTwits([FromRoute]int id)
        {
            _logger.LogInformation($"Returning all personal twits for user {id}.");

            return Ok(_twitContext.Messages.ToList().Where(msg => msg.AuthorId.Equals(id)));
        }

        [HttpPut("mark-message")]
        public async Task<ActionResult<string>> MarkMessage([FromBody]FlaggingInput input)
        {
            _logger.LogInformation($"Mark message endpoint was called on msg id: {input.MessageId}.");

            var flaggedMsg = _twitContext.Messages.FirstOrDefault(msg => msg.MessageId.Equals(input.MessageId));

            if (flaggedMsg != null)
            {
                flaggedMsg.Flagged = input.FlagMessage;
                _twitContext.SaveChanges();

                return Ok();
            }
            else
            {
                return BadRequest();
            }

        }
    }
}
