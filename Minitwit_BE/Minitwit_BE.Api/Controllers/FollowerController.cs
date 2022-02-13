using Microsoft.AspNetCore.Mvc;
using Minitwit_BE.Domain;
using Minitwit_BE.Persistence;

namespace Minitwit_BE.Api.Controllers
{
    [ApiController]
    [Route("api/followers")]
    public class FollowerController : ControllerBase
    {
        private readonly TwitContext _twitContext;          // To dependency inject the context instance.
        private readonly ILogger<FollowerController> _logger;

        public FollowerController(TwitContext twitContext, ILogger<FollowerController> logger)
        {
            _twitContext = twitContext;
            _logger = logger;
        }

        [HttpGet("list/{id}")]
        public async Task<ActionResult<List<Follower>>> GetFollowedUsers([FromRoute]int id)
        {
            _logger.LogInformation($"GetFollowedUsers endpoint was called for id: {id}");

            var followersList = _twitContext.Followers.Where(entry => entry.WhoId.Equals(id)).ToList();

            return Ok(followersList);
        }


        [HttpPost("follow")]
        public async Task<ActionResult> follow([FromBody]FollowerInput input)
        {
            _logger.LogInformation($"Follow endpoint was called with whomid: {input.WhomId}, by: {input.WhoId}.");

            var newFollower = new Follower
            {
                WhoId = input.WhoId,
                WhomId = input.WhomId
            };

            _twitContext.Add(newFollower);
            _twitContext.SaveChanges();
            return Ok();
        }

        [HttpDelete("unfollow/{id}")]
        public async Task<ActionResult> Unfollow([FromRoute]int id)
        {
            _logger.LogInformation($"Unfollow endpoint was called for id: {id}");

            var deletedFollow = _twitContext.Followers.FirstOrDefault(entry => entry.Id.Equals(id));

            if (deletedFollow != null)
            {
                _twitContext.Remove(deletedFollow);
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