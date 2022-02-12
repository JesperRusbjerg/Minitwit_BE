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
            User? existingUser = _twitContext.Users.SingleOrDefault(user => user.UserName == input.UserName || user.Email == input.Email);
            if (existingUser != null) {
                return BadRequest("User with that nickname or email already exists!");
            }
            string hashedPw = input.applyHash();
            User user = new User
            {
                UserName = input.UserName,
                Email = input.Email,
                PwHash = hashedPw,
            };

            _twitContext.Add(user);
            _twitContext.SaveChanges();
            return Ok(user);
        }

        [HttpGet("login")]
        public async Task<ActionResult<int>> login([FromBody]UserInput input)
        {
            User? authUser = _twitContext.Users.SingleOrDefault(user => user.Email == input.Email);
            if (authUser != null)
            {   
                Boolean hashedPw = authUser.compareHash(input.Password);
                if (hashedPw)
                {
                    return Ok(authUser.UserId);
                }
                else
                {
                    return Unauthorized();
                }
            }
            else
            {
                return Unauthorized();
            }
        }
    }
}