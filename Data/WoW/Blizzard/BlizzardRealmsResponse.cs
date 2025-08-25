using Newtonsoft.Json;

namespace dev_library.Data.WoW.Blizzard
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class ConnectedRealm
    {
        [JsonProperty("href")]
        private string href;

        public string Href { get => href[..href.LastIndexOf('?')]; set => href = value; }
    }

    public class Links
    {
        [JsonProperty("self")]
        public Self Self;
    }

    public class BlizzardRealmsResponse
    {
        [JsonProperty("_links")]
        public Links Links;

        [JsonProperty("connected_realms")]
        public List<ConnectedRealm> ConnectedRealms;
    }

    public class Self
    {
        [JsonProperty("href")]
        public string Href;
    }


}
