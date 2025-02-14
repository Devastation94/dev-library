using Newtonsoft.Json;
using System.Text.Json;

namespace dev_refined.Data
{
    public class DiscordRequest
    {
        public DiscordRequest(string content)
        {
            Content = content;
        }

        [JsonProperty("content")]
        public string Content { get; set; }
    }
}
