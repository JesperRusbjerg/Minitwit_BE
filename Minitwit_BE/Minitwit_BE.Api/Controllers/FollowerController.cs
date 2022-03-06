using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Minitwit_BE.Api.Dtos;
using Minitwit_BE.Domain;
using Minitwit_BE.DomainService.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Minitwit_BE.Api.Controllers
{
    [ApiController]
    [Route("api/followers")]
    public class FollowerController : ControllerBase
    {
        private readonly ILogger<FollowerController> _logger;
        private readonly IFollowerDomainService _followerService;

        public FollowerController(ILogger<FollowerController> logger, IFollowerDomainService followerService)
        {
            _logger = logger;
            _followerService = followerService;
        }

        [HttpGet("list/{id}")]
        public async Task<ActionResult<List<Follower>>> GetFollowedUsers([FromRoute] int id)
        {
            ValidateId(id);

            _logger.LogInformation($"GetFollowedUsers endpoint was called for id: {id}");

            var followers = await _followerService.GetFollowedUsers(id);

            return Ok(followers);
        }


        [HttpPost("follow")]
        public async Task<ActionResult> Follow([FromBody] FollowerDto input)
        {
            ValidateFollowerDto(input);

            _logger.LogInformation($"Follow endpoint was called with whomid: {input.WhomId}, by: {input.WhoId}.");

            var newFollower = new Follower
            {
                WhoId = input.WhoId,
                WhomId = input.WhomId
            };

            await _followerService.Follow(newFollower);

            return Ok(newFollower);
        }

        [HttpDelete("unfollow/{id}")]
        public async Task<ActionResult> Unfollow([FromRoute] int id)
        {
            ValidateId(id);

            _logger.LogInformation($"Unfollow endpoint was called for id: {id}");

            await _followerService.UnFollow(id);

            return Ok();
        }

        #region PrivateMethods
        private void ValidateFollowerDto(FollowerDto input)
        {
            if (input == null || input.WhomId < 0 || input.WhoId < 0)
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
        #endregion
    }
}