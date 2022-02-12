using Microsoft.AspNetCore.Mvc;
using Minitwit_BE.Domain;
using Minitwit_BE.Persistence;

namespace Minitwit_BE.Api.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly TwitContext _twitContext;

        public UserController(TwitContext twitContext)
        {
            _twitContext = twitContext;
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> registerUser([FromBody]UserInput input)
        {
            var existingUser = _twitContext.Users.FirstOrDefault(user => user.UserName.Equals(input.UserName) || user.Email.Equals(input.Email));
            if (existingUser != null) {
                return BadRequest("User with that nickname or email already exists!");
            }

            var user = new User
            {
                UserName = input.UserName,
                Email = input.Email,
                PwHash = input.PwHash,
            };

            _twitContext.Add(user);
            _twitContext.SaveChanges();
            return Ok(user);
        }

        [HttpGet("login")]
        public async Task<ActionResult<int>> login([FromBody]UserInput input)
        {
            var authUser = _twitContext.Users.FirstOrDefault(user => user.Email.Equals(input.Email) && user.PwHash.Equals(input.PwHash));
            if (authUser != null)
            {   
                return Ok(authUser.UserId);
            }
            else
            {
                return Unauthorized();
            }
        }
    }
}