using Newtonsoft.Json;

namespace Minitwit_BE.Api.Dtos.Simulation
{
    public class GetMessageDto
    {
        [JsonProperty("content")]
        public string? Text { get; set; }

        [JsonProperty("pub_date")]
        public string? PublishDate { get; set; }

        [JsonProperty("user")]
        public string? UserName { get; set; }
    }
}