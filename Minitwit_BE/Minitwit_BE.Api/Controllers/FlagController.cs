using Microsoft.AspNetCore.Mvc;
using Minitwit_BE.Domain;
using Minitwit_BE.Persistence;

namespace Minitwit_BE.Api.Controllers
{
    [ApiController]
    [Route("api/flag")]
    public class FlagController : ControllerBase
    {
        private readonly TwitContext _twitContext;          // To dependency inject the context instance.
        
        public FlagController(TwitContext twitContext)
        {
            _twitContext = twitContext;
        }

        [HttpGet("test")]
        public async Task<string> TestEndpoint()
        {
            return await Task.FromResult("test");
        }

        [HttpPut("mark-message/{id}/{followAction}")]
        public async Task<ActionResult<string>> markMessage(int id, int followAction)
        {
            Message? flaggedMsg = _twitContext.Messages.SingleOrDefault(msg => msg.MessageId == id);
            if (flaggedMsg != null)
            {
                if (followAction == 1)
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
