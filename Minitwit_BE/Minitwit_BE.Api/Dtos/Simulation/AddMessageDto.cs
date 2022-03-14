using Newtonsoft.Json;

namespace Minitwit_BE.Api.Dtos.Simulation
{
    public class AddMessageDto
    {
        [JsonProperty("content")]
        public string? Content { get; set; }
    }
}
