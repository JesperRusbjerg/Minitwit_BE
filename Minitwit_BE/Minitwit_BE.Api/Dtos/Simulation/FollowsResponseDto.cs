using Newtonsoft.Json;

namespace Minitwit_BE.Api.Dtos.Simulation
{
    public class FollowsResponseDto
    {
        [JsonProperty("follows")]
        public List<String>? Follows { get; set; }
    }
}