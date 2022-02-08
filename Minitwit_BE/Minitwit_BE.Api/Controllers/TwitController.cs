using Microsoft.AspNetCore.Mvc;

namespace Minitwit_BE.Api.Controllers
{
    [ApiController]
    [Route("api/twit")]
    public class TwitController : ControllerBase
    {
        [HttpGet("test")]
        public async Task<string> TestEndpoint()
        {
            return await Task.FromResult("test");
        }
    }
}
