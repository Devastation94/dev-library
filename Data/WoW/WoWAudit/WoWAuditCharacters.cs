using Newtonsoft.Json;

namespace dev_refined.Data
{
    public class WoWAuditCharacter
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("realm")]
        public string Realm { get; set; }

        [JsonProperty("class")]
        public string Class { get; set; }

        [JsonProperty("role")]
        public string Role { get; set; }

        [JsonProperty("rank")]
        public string Rank { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("note")]
        public string Note { get; set; }

        [JsonProperty("blizzard_id")]
        public int BlizzardId { get; set; }

        [JsonProperty("tracking_since")]
        public DateTime TrackingSince { get; set; }
    }
}
