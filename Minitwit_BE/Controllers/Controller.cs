using Microsoft.AspNetCore.Mvc;

namespace MinitwitBE.Api.Controllers
{
    [ApiController]
    [Route("api/")]
    public class Controller : ControllerBase
    {
        [HttpGet("test")]
        public async Task<string> TestEndpoint()
        {
            return await Task.FromResult("test");
        }
    }
}
