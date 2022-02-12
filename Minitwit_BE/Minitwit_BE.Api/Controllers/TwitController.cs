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
        
        public TwitController(TwitContext twitContext)
        {
            _twitContext = twitContext;
        }
        
        [HttpGet("test")]
        public async Task<string> TestEndpoint()
        {
            return await Task.FromResult("test");
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddTwit([FromBody]MessageInput input)
        {
            // Add new twit
            Console.WriteLine("Inserting a new twit");
            // Primary keys should be auto incremented when you add entity to the table and dont explicitely specify specify the ID
            var msg = new Message 
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
            Console.WriteLine("Reading");
            return Ok(_twitContext.Messages.ToList());
        }

        [HttpGet("personal-twits/{id}")]
        public async Task<ActionResult<List<Message>>> GetPersonalTwits([FromRoute]int id)
        {
            Console.WriteLine("Reading");
            return Ok(_twitContext.Messages.ToList().Where(msg => msg.AuthorId.Equals(id)));
        }

        [HttpPut("mark-message")]
        public async Task<ActionResult<string>> markMessage([FromBody]FlaggingInput input)
        {
            var flaggedMsg = _twitContext.Messages.FirstOrDefault(msg => msg.MessageId.Equals(input.MessageId));
            if (flaggedMsg != null)
            {
                if (input.FlagMessage)
                {
                    flaggedMsg.Flagged = true;
                }
                else
                {
                    flaggedMsg.Flagged = false;
                }
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
