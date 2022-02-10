using Microsoft.AspNetCore.Mvc;
using Minitwit_BE.Domain;
using Minitwit_BE.Persistence;

namespace Minitwit_BE.Api.Controllers
{
    [ApiController]
    [Route("/api/user")]
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
            // TODO: hash the password
            User user = new User
            {
                UserName = input.UserName,
                Email = input.Email,
                PwHash = input.Password
            };

            // TODO: handle if username/email exists
            _twitContext.Add(user);
            _twitContext.SaveChanges();
            return Ok(user);
        }
    }
}