using Newtonsoft.Json;

namespace Minitwit_BE.Api.Dtos.Simulation
{
    public class FollowedUserDto
    {
        [JsonProperty("username")]
        public string? UserName { get; set; }
    }
}