using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Minitwit_BE.Api.Health
{
    [ApiController]
    [Route("")]
    public class HealthController : ControllerBase
    {
        private readonly ILogger<HealthController> _logger;

        public HealthController(ILogger<HealthController> logger)
        {
            _logger = logger;
        }

        [HttpGet("/health")]
        public async Task<string> HealthEndpoint()
        {
            _logger.LogDebug("Health endpoint was called!");

            return await Task.FromResult($"{DateTime.Now}: I'm a healthy big boi!");
        }
    }
}
