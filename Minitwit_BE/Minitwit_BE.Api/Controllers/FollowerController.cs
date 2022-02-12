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
        public async Task<ActionResult<List<Follower>>> getFollowedUsers(int id)
        {
            List <Follower> followersList = _twitContext.Followers.Where(entry => entry.WhoId == id).ToList();
            return Ok(followersList);
        }


        [HttpPost("follow")]
        public async Task<ActionResult> follow([FromBody]FollowerInput input)
        {
            Follower newFollower = new Follower
            {
                WhoId = input.WhoId,
                WhomId = input.WhomId
            };

            _twitContext.Add(newFollower);
            _twitContext.SaveChanges();
            return Ok();
        }

        [HttpDelete("unfollow/{id}")]
        public async Task<ActionResult<string>> unfollow(int id)
        {
            Follower? deletedFollow = _twitContext.Followers.SingleOrDefault(entry => entry.Id == id);
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