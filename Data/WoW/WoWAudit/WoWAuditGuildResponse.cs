using Newtonsoft.Json;

namespace dev_refined.Data
{
    public class Character
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("race")]
        public string Race { get; set; }

        [JsonProperty("class")]
        public string Class { get; set; }

        [JsonProperty("active_spec_name")]
        public string ActiveSpecName { get; set; }

        [JsonProperty("active_spec_role")]
        public string ActiveSpecRole { get; set; }

        [JsonProperty("gender")]
        public string Gender { get; set; }

        [JsonProperty("faction")]
        public string Faction { get; set; }

        [JsonProperty("achievement_points")]
        public int AchievementPoints { get; set; }

        [JsonProperty("honorable_kills")]
        public int HonorableKills { get; set; }

        [JsonProperty("region")]
        public string Region { get; set; }

        [JsonProperty("realm")]
        public string Realm { get; set; }

        [JsonProperty("last_crawled_at")]
        public DateTime LastCrawledAt { get; set; }

        [JsonProperty("profile_url")]
        public string ProfileUrl { get; set; }

        [JsonProperty("profile_banner")]
        public string ProfileBanner { get; set; }
    }

    public class Member
    {
        [JsonProperty("rank")]
        public int Rank { get; set; }

        [JsonProperty("character")]
        public Character Character { get; set; }
    }

    public class WoWAuditGuildResponse
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("faction")]
        public string Faction { get; set; }

        [JsonProperty("region")]
        public string Region { get; set; }

        [JsonProperty("realm")]
        public string Realm { get; set; }

        [JsonProperty("last_crawled_at")]
        public DateTime LastCrawledAt { get; set; }

        [JsonProperty("profile_url")]
        public string ProfileUrl { get; set; }

        [JsonProperty("members")]
        public List<Member> Members { get; set; }
    }

}
