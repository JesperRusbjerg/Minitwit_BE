using Newtonsoft.Json;

namespace Minitwit_BE.Api.Dtos.Simulation
{
    public class AddMessageDto
    {
        [JsonProperty("content")]
        public string Text { get; set; }
    }
}
