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
        
        public FollowerController(TwitContext twitContext)
        {
            _twitContext = twitContext;
        }

        [HttpGet("list/{id}")]
        public async Task<ActionResult<List<Follower>>> getFollowedUsers([FromRoute]int id)
        {
            var followersList = _twitContext.Followers.Where(entry => entry.WhoId.Equals(id)).ToList();
            return Ok(followersList);
        }


        [HttpPost("follow")]
        public async Task<ActionResult> follow([FromBody]FollowerInput input)
        {
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
        public async Task<ActionResult> unfollow([FromRoute]int id)
        {
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