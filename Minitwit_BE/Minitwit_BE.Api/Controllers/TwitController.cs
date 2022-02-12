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
        public async Task<ActionResult<Message>> AddTwit([FromBody]MessageInput input)
        {
            // Add new twit
            Console.WriteLine("Inserting a new twit");
            // Primary keys should be auto incremented when you add entity to the table and dont explicitely specify specify the ID
            Message msg = new Message 
            { 
                AuthorId = input.AuthorId,
                Flagged = false,
                PublishDate = DateTime.Now,
                Text = input.Text
            };
            
            _twitContext.Add(msg);
            _twitContext.SaveChanges();
            return Ok(msg);
        }

        [HttpGet("public-twits")]
        public async Task<ActionResult<List<Message>>> GetTwits()
        {
            Console.WriteLine("Reading");
            return Ok(_twitContext.Messages.ToList());
        }

        [HttpGet("personal-twits/{id}")]
        public async Task<ActionResult<List<Message>>> GetPersonalTwits(int id)
        {
            Console.WriteLine("Reading");
            return Ok(_twitContext.Messages.ToList().Where(msg => msg.AuthorId == id));
        }

        [HttpPut("mark-message")]
        public async Task<ActionResult<string>> markMessage([FromBody]FlaggingInput input)
        {
            Message? flaggedMsg = _twitContext.Messages.SingleOrDefault(msg => msg.MessageId == input.MessageId);
            if (flaggedMsg != null)
            {
                if (input.FlaggingAction == 1)
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
