using Newtonsoft.Json;

namespace dev_refined.Data.Realms
{
    public class BlizzardOAuthResponse
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);

        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty("sub")]
        public string Sub { get; set; }



    }
}
