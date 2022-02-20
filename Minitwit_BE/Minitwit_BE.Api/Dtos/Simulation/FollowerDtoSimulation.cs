using Newtonsoft.Json;

namespace Minitwit_BE.Api.Dtos.Simulation
{
    public class FollowerDtoSimulation
    {
        [JsonProperty("follow")]
        public string? Follow { get; set; }
        [JsonProperty("unfollow")]
        public string? Unfollow { get; set; }
    }
}