using Microsoft.AspNetCore.Mvc;
using Minitwit_BE.Api.Dtos;
using Minitwit_BE.Domain;
using Minitwit_BE.DomainService.Interfaces;

namespace Minitwit_BE.Api.Controllers
{
    [ApiController]
    [Route("api/followers")]
    public class FollowerController : ControllerBase
    {
        private readonly ILogger<FollowerController> _logger;
        private readonly IFollowerDomainService _followerService;
        private readonly IUserDomainService _userDomainService;

        public FollowerController(ILogger<FollowerController> logger, IFollowerDomainService followerService, IUserDomainService userDomainService)
        {
            _logger = logger;
            _followerService = followerService;
            _userDomainService = userDomainService;
        }

        [HttpGet("list/{id}")]
        public async Task<ActionResult<List<Follower>>> GetFollowedUsers([FromRoute] int id)
        {
            ValidateId(id);

            _logger.LogInformation($"GetFollowedUsers endpoint was called for id: {id}");

            var followers = await _followerService.GetFollowedUsers(id);


            List<FollowerDtoList> list = new List<FollowerDtoList>();

            for (int i = 0; i < followers.Count(); i++)
            {
                User user = await _userDomainService.GetUserById(followers.ElementAt(i).Id);
 
                FollowerDtoList dto = new FollowerDtoList();
                dto.Name = user.UserName;
                dto.Email = user.Email;
                dto.Id = followers.ElementAt(i).Id;
                dto.WhoId = followers.ElementAt(i).WhoId;
                dto.WhomId = followers.ElementAt(i).WhomId;
                list.Add(dto);
            }

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

        private class FollowerDtoList
        {
            public int Id { get; set; }
            public int WhoId { get; set; }
            public int WhomId { get; set; }

            public String Name { get; set; }

            public String Email { get; set; }
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