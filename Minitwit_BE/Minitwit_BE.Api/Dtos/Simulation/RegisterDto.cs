using Newtonsoft.Json;

namespace Minitwit_BE.Api.Dtos.Simulation
{
    public class RegisterDto
    {
        [JsonProperty("username")]
        public string? UserName { get; set; }

        [JsonProperty("pwd")]
        public string? Password { get; set; }

        [JsonProperty("email")]
        public string? Email { get; set; }

    }
}