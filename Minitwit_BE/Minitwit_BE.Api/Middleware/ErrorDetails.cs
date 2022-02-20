using Newtonsoft.Json;
using System.Text.Json;

namespace Minitwit_BE.Api.Middleware
{
    internal class ErrorDetails
    {
        [JsonProperty("status")]
        public int StatusCode { get; set; }
        [JsonProperty("error_msg")]
        public string Message { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
