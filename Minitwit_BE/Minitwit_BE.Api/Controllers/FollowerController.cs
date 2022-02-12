using Microsoft.AspNetCore.Mvc;
using Minitwit_BE.Api.Dtos;
using Minitwit_BE.Domain;
using Minitwit_BE.DomainService;
using Minitwit_BE.Persistence;

namespace Minitwit_BE.Api.Controllers
{
    [ApiController]
    [Route("api/followers")]
    public class FollowerController : ControllerBase
    {
        private readonly ILogger<FollowerController> _logger;
        private readonly FollowerDomainService _followerService;

        public FollowerController(ILogger<FollowerController> logger, FollowerDomainService followerService)
        {
            _logger = logger;
            _followerService = followerService;
        }

        [HttpGet("list/{id}")]
        public async Task<ActionResult<List<Follower>>> GetFollowedUsers([FromRoute]int id)
        {
            // Validate inputs
            
            _logger.LogInformation($"GetFollowedUsers endpoint was called for id: {id}");

            return Ok(_followerService.GetFollowedUsers(id));
        }


        [HttpPost("follow")]
        public async Task<ActionResult> Follow([FromBody]FollowerDto input)
        {
            // Validate inputs

            _logger.LogInformation($"Follow endpoint was called with whomid: {input.WhomId}, by: {input.WhoId}.");

            var newFollower = new Follower
            {
                WhoId = input.WhoId,
                WhomId = input.WhomId
            };

            await _followerService.Follow(newFollower);

            return Ok();
        }

        [HttpDelete("unfollow/{id}")]
        public async Task<ActionResult> Unfollow([FromRoute]int id)
        {
            // Validate inputs

            _logger.LogInformation($"Unfollow endpoint was called for id: {id}");

            await _followerService.UnFollow(id);

            return Ok();
        }
    }
}